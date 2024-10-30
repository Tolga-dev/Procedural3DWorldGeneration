using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerators
{
    public class SierpinskiTriangle : MonoBehaviour
    {
        public int iterations = 7;
        public float segmentLength = 1;
        public GameObject segmentPrefab;
        
        private void Start()
        {
            string axiom = "F−G−G";
            
            var rules = new Dictionary<char, string>
            {
                { 'F', "F−G+F+G−F" },
                { 'G', "GG" }
            };

            string result = AlgaeGenerate(axiom, rules, iterations);
            VisualizeSierpinskiTriangle(result);
            Debug.Log($"After {iterations} iterations: {result}");
        }
        public string AlgaeGenerate(string axiom, Dictionary<char, string> rules, int iteration)
        {
            string currentString = axiom;

            for (int i = 0; i < iteration; i++)
            {
                string nextString = "";

                foreach (char c in currentString)
                {
                    nextString += rules.TryGetValue(c, out var rule) ? rule : c.ToString();
                }

                currentString = nextString;
            }

            return currentString;
        }
        
        void VisualizeSierpinskiTriangle(string result)
        {
            Vector3 currentPosition = Vector3.zero;
            Quaternion currentRotation = Quaternion.identity;

            foreach (char command in result)
            {
                if (command == 'F' || command == 'G')
                {
                    GameObject segment = Instantiate(segmentPrefab, currentPosition, currentRotation);
                    segment.transform.localScale = new Vector3(0.5f, 0.5f,segmentLength); // Adjust scale if needed
                    currentPosition += currentRotation * Vector3.right * segmentLength; // Move forward
                }
                else if (command == '+')
                {
                    currentRotation *= Quaternion.Euler(0, 120, 0);
                }
                else if (command == '−')
                {
                    currentRotation *= Quaternion.Euler(0, -120,0);
                }
            }
        }
    }
}