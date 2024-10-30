using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace WorldGenerators
{
    public class FractalBinaryTree : MonoBehaviour
    {
        public int iterations = 7;
        public GameObject branchPrefab;
        public GameObject leafPrefab;
        
        private void Start()
        {
            string axiom = "0";
            
            var rules = new Dictionary<char, string>
            {
                { '1', "11" },
                { '0', "1[0]0" }
            };

            string result = AlgaeGenerate(axiom, rules, iterations);
            DrawTree(result);
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

        public void DrawTree(string treeString)
        {
            Stack<(Vector3 position, Quaternion rotation)> stack = new Stack<(Vector3 position, Quaternion rotation)>();
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float length = 1f;

            foreach (var letter in treeString)
            {
                if (letter == '1')
                {
                    Vector3 newPosition = position + rotation * Vector3.up * length;
                    GameObject branch = Instantiate(branchPrefab, position, rotation);
                    branch.transform.localScale = new Vector3(0.1f, length, 0.1f);
                    position = newPosition;
                }
                else if (letter == '0')
                {
                    Vector3 newPosition = position + rotation * Vector3.up * length;
                    GameObject branch = Instantiate(leafPrefab, position, rotation);
                    branch.transform.localScale = new Vector3(0.1f, length, 0.1f);
                    position = newPosition;
                }
                else if (letter == '[')
                {
                    stack.Push((position, rotation));
                    rotation *= Quaternion.Euler(0, 0, 25);
                    
                }
                else if (letter == ']')
                {
                    (position, rotation) = stack.Pop();
                    rotation *= Quaternion.Euler(0, 0, -25);
                }
                
            }
            
        }

    }
}
