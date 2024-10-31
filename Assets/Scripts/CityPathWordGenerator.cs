using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CityPathWordGenerator : MonoBehaviour
{
    public GameObject roadSegmentPrefab;    // Prefab for a road segment
    public int iterations = 4;              // Number of iterations for the L-system
    public float segmentLength = 5f;        // Length of each road segment
    public float angle = 90f;               // Angle for turns in degrees (90 for grid)

    private string axiom = "F";             // Starting point of the L-system
    private Dictionary<char, string> rules = new Dictionary<char, string>(); // Rules dictionary
    private List<Vector3> usedPositions = new List<Vector3>(); // Track used positions

    private void Start()
    {
        rules['F'] = "F+F−F−F+F"; // Define L-system rule for road generation
        string roadPath = GenerateLSystem();
        Debug.Log(roadPath);
        DrawRoads(roadPath);
    }

    private string GenerateLSystem()
    {
        StringBuilder currentString = new StringBuilder(axiom);

        // Apply L-system rules for the specified number of iterations
        for (int i = 0; i < iterations; i++)
        {
            StringBuilder newString = new StringBuilder();

            foreach (char c in currentString.ToString())
            {
                newString.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = newString;
        }

        return currentString.ToString();
    }

    private void DrawRoads(string roadPath)
    {
        Stack<(Vector3 position, Quaternion rotation)> stack = new Stack<(Vector3, Quaternion)>();
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;

        foreach (char instruction in roadPath)
        {
            if (instruction == 'F')
            {
                // Move forward and place a road segment
                Vector3 newPosition = currentPosition + currentRotation * Vector3.forward * segmentLength;

                // Avoid overlapping by checking used positions
                if (!usedPositions.Contains(newPosition))
                {
                    Instantiate(roadSegmentPrefab, currentPosition, currentRotation);
                    usedPositions.Add(newPosition);
                    currentPosition = newPosition;
                }
            }
            else if (instruction == '+')
            {
                // Turn left
                currentRotation *= Quaternion.Euler(0, angle, 0);
            }
            else if (instruction == '-')
            {
                // Turn right
                currentRotation *= Quaternion.Euler(0, -angle, 0);
            }
            else if (instruction == '[')
            {
                // Save current position and rotation
                stack.Push((currentPosition, currentRotation));
            }
            else if (instruction == ']')
            {
                // Restore last saved position and rotation
                (currentPosition, currentRotation) = stack.Pop();
            }
        }
    }

    // Optional: Draws gizmos to visualize road path in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        for (int i = 0; i < usedPositions.Count - 1; i++)
        {
            Gizmos.DrawLine(usedPositions[i], usedPositions[i + 1]);
        }
    }
}