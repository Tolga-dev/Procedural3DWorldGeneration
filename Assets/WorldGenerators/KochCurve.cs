using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerators
{
    public class KochCurve : MonoBehaviour
    {
        public int iterations = 7;
        public GameObject segmentPrefab;
        public float segmentLength = 1.0f; 

        void Start()
        {
            string axiom = "F";
            var rules = new Dictionary<char, string>
            {
                { 'F', "F+F−F−F+F" }
            };

            // Generate the fractal string
            string result = GenerateLSystem(axiom, rules, iterations);
            VisualizeKochCurve(result);
        }

        string GenerateLSystem(string axiom, Dictionary<char, string> rules, int iteration)
        {
            string currentString = axiom;

            for (int i = 0; i < iteration; i++)
            {
                string nextString = "";
                foreach (char c in currentString)
                {
                    nextString += rules.ContainsKey(c) ? rules[c] : c.ToString();
                }
                currentString = nextString;
            }

            return currentString;
        }


        void VisualizeKochCurve(string result)
        {
            Vector3 currentPosition = Vector3.zero;
            Quaternion currentRotation = Quaternion.identity;

            foreach (char command in result)
            {
                if (command == 'F')
                {
                    GameObject segment = Instantiate(segmentPrefab, currentPosition, currentRotation);
                    segment.transform.localScale = new Vector3(0.5f, 0.5f,segmentLength); // Adjust scale if needed
                    currentPosition += currentRotation * Vector3.right * segmentLength; // Move forward
                }
                else if (command == '+')
                {
                    currentRotation *= Quaternion.Euler(0, 90, 0);
                }
                else if (command == '−')
                {
                    currentRotation *= Quaternion.Euler(0, -90,0);
                }
            }
        }
    }
}