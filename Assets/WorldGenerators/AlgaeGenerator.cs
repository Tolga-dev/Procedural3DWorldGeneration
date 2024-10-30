using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerators
{
    public class AlgaeGenerator : MonoBehaviour
    {
        public int iterations = 7;

        private void Start()
        {
            string axiom = "A";
            var rules = new Dictionary<char, string>
            {
                { 'A', "AB" },
                { 'B', "A" }
            };


            string result = AlgaeGenerate(axiom, rules, iterations);
            Debug.Log($"After {iterations} iterations: {result}");
        }
        
        public string AlgaeGenerate(string axiom, Dictionary<char, string> rules, int iterations)
        {
            string currentString = axiom;

            for (int i = 0; i < iterations; i++)
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
    }
}