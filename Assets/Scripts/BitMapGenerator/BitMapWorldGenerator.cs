using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BitMapGenerator
{
    public class BitMapWorldGenerator : MonoBehaviour
    {
        public Transform generationStartPoint;
        public float offset;
        public GameObject groundObject;
        
        public GameObject tripleRoadObject;
        public GameObject forthRoadObject;
        public GameObject twoRoadObject;
        public GameObject cornerRoadObject;
        
        public Vector2Int sizeOfGridWorld;
        public List<BuildCells> buildCells = new List<BuildCells>();
        public List<GameObject> buildPrefabs = new List<GameObject>();
        
        private const int HOUSE_CODE = 0;
        private const int ROAD_CODE = 1;

        private void Start()
        {   
            GenerateWorld();
            PrintWorldGrid();
            VisualizeWorld();
        }
        
        private void GenerateWorld()
        {
            for (int x = 0; x < sizeOfGridWorld.x; x++)
            {
                var buildCellsInRow = GenerateRowCells(x);
                buildCells.Add(new BuildCells { cells = buildCellsInRow });
            }

            for (int i = 0; i < buildCells.Count; i++)
            {
                if (i % 3 == 1)
                {
                    PlaceRoadsInRow(i);
                }
            }
        }

        private List<BuildCell> GenerateRowCells(int x)
        {
            var buildCellsInRow = new List<BuildCell>();

            bool isRoadRow = (x % 3 == 0);
            for (int y = 0; y < sizeOfGridWorld.y; y++)
            {
                int code = DetermineCellCode(isRoadRow, y);
                buildCellsInRow.Add(new BuildCell { code = code });
            }

            return buildCellsInRow;
        }

        private int DetermineCellCode(bool isRoad, int y)
        {
            if (isRoad || y == 0 || y == sizeOfGridWorld.y - 1)
            {
                return ROAD_CODE;
            }

            return HOUSE_CODE;
        }
        
        

        private void PlaceRoadsInRow(int rowIndex)
        {
            var maxRoadCount = CalculateMaxReplacements(buildCells[0].cells.Count);
            int roadCount = Random.Range(1, maxRoadCount);
            var availableIndexes = GetMiddleIndexes();

            for (int i = 0; i < roadCount; i++)
            {
                if(availableIndexes.Count == 0)
                    break;
                
                RemoveHouseForRoad(availableIndexes, rowIndex);
            }
        }

        private List<int> GetMiddleIndexes()
        {
            var middleIndexes = new List<int>();
            for (int j = 2; j < buildCells[0].cells.Count - 2; j++)
            {
                middleIndexes.Add(j);
            }

            return middleIndexes;
        }

        private void RemoveHouseForRoad(List<int> availableIndexes, int rowIndex)
        {
            int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count-1)];

            buildCells[rowIndex].cells[randomIndex].code = ROAD_CODE;
            buildCells[rowIndex + 1].cells[randomIndex].code = ROAD_CODE;

            RemoveNearbyIndexes(availableIndexes, randomIndex);
        }

        private void RemoveNearbyIndexes(List<int> availableIndexes, int index)
        {
            availableIndexes.Remove(index);
            availableIndexes.Remove(index+1);
            availableIndexes.Remove(index-1);
        }

        private int CalculateMaxReplacements(int bitCount)
        {
            return (bitCount % 2 == 0) ? (bitCount / 2) : (bitCount / 2 + 1);
        }
        
        private void VisualizeWorld()
        {
            
            var randRots = new List<float>() {0, 90, 180, 270};
            
            for (int i = 0; i < buildCells.Count; i++)
            {
                for (int j = 0; j < buildCells[i].cells.Count; j++)
                {
                    var code = buildCells[i].cells[j].code;
                    Vector3 spawnPosition = generationStartPoint.position + new Vector3(i * offset, 0, j * offset);
   
                    if (code == 0)
                    {
                        var ground = Instantiate(groundObject, spawnPosition, Quaternion.identity);
                        var house = Instantiate(buildPrefabs[Random.Range(0, buildPrefabs.Count)], ground.transform.position, 
                            Quaternion.Euler(0,randRots[Random.Range(0, randRots.Count)],0));
                    }
                    else
                    {

                        bool hasLeftNeighbor = (j > 0) && (buildCells[i].cells[j - 1].code == ROAD_CODE);
                        bool hasRightNeighbor = (j < buildCells[i].cells.Count - 1) && (buildCells[i].cells[j + 1].code == ROAD_CODE);
                        bool hasTopNeighbor = (i > 0) && (buildCells[i - 1].cells[j].code == ROAD_CODE);
                        bool hasBottomNeighbor = (i < buildCells.Count - 1) && (buildCells[i + 1].cells[j].code == ROAD_CODE);

                        int roadNeighbors = (hasLeftNeighbor ? 1 : 0) + (hasRightNeighbor ? 1 : 0) + (hasTopNeighbor ? 1 : 0) + (hasBottomNeighbor ? 1 : 0);

                        if (roadNeighbors == 3)
                        {
                            var road = Instantiate(tripleRoadObject, spawnPosition, Quaternion.identity);

                            if (!hasLeftNeighbor) road.transform.rotation = Quaternion.Euler(0, 270, 0);      // "T" shape facing right
                            else if (!hasRightNeighbor) road.transform.rotation = Quaternion.Euler(0, 90, 0); // "T" shape facing left
                            else if (!hasTopNeighbor) road.transform.rotation = Quaternion.Euler(0, 0, 0);   // "T" shape facing down
                            else if (!hasBottomNeighbor) road.transform.rotation = Quaternion.Euler(0, 180, 0); // "T" shape facing up
                        }
                        else if (roadNeighbors == 4)
                        {
                            var road = Instantiate(forthRoadObject, spawnPosition, Quaternion.identity);
                            road.transform.rotation = Quaternion.identity; // Cross, no rotation needed
                        }
                        else
                        {
                            
                            if (i == 0 && j == 0) // lu
                            {
                                var road = Instantiate(cornerRoadObject, spawnPosition, Quaternion.identity);
                                road.transform.rotation = Quaternion.Euler(0, 90, 0); // Adjust rotation as needed
                            }
                            else if (i == 0 && j == buildCells[i].cells.Count - 1) // ru
                            {
                                var road = Instantiate(cornerRoadObject, spawnPosition, Quaternion.identity);
                                road.transform.rotation = Quaternion.Euler(0, 180, 0); // Adjust rotation as needed
                            }
                            else if (i == buildCells.Count - 1 && j == 0) // ld
                            {
                                var road = Instantiate(cornerRoadObject, spawnPosition, Quaternion.identity);
                                road.transform.rotation = Quaternion.Euler(0, 0, 0); // Adjust rotation as needed
                            }
                            else if (i == buildCells.Count - 1 && j == buildCells[i].cells.Count - 1) // rd
                            {
                                var road = Instantiate(cornerRoadObject, spawnPosition, Quaternion.identity);
                                road.transform.rotation = Quaternion.Euler(0, 270, 0); // Adjust rotation as needed
                            }
                            else
                            {
                                var road = Instantiate(twoRoadObject, spawnPosition, Quaternion.identity);
                                bool isRoadRow = (i % 3 == 0);
                                if (isRoadRow && (j != 0 || j != buildCells[i].cells.Count - 1))
                                {
                                    road.transform.rotation = Quaternion.Euler(0,90,0);
                                }
                            }
                        }
                    }
                }
                
            }


       
            
        }


        private void PrintWorldGrid()
        {
            var gridOutput = new System.Text.StringBuilder();
            foreach (var row in buildCells)
            {
                foreach (var cell in row.cells)
                {
                    gridOutput.Append(cell.code);
                }

                gridOutput.AppendLine();
            }

            Debug.Log(gridOutput.ToString());
        }
    }

    [Serializable]
    public class BuildCells
    {
        public List<BuildCell> cells;
    }

    [Serializable]
    public class BuildCell
    {
        public int code;
    }
}