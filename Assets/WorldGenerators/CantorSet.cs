using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerators
{
    public class CantorSet : MonoBehaviour
    {
        public int iterations = 7;
        public GameObject segmentPrefab;
        public float segmentLength = 1.0f; 

        private void Start()
        {
            string axiom = "A";
            var rules = new Dictionary<char, string>
            {
                { 'A', "ABA" },
                { 'B', "BBBB" }
            };

            string result = AlgaeGenerate(axiom, rules, iterations);
            VisualizeCantorSet(result);
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

        public void VisualizeCantorSet(string result)
        {
            Vector3 currentPos = Vector3.zero;

            foreach (var letter in result)
            {
                if (letter == 'A')
                {
                    Instantiate(segmentPrefab, currentPos, Quaternion.identity);
                }
                currentPos += Vector3.right * segmentLength;
            }
        }
    }
}