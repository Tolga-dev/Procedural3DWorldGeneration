using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BitMapGenerator
{
    public class BitMapWorldGenerator : MonoBehaviour
    {
        public Transform generationStartPoint;
        public Vector2Int sizeOfGridWorld;
        public float offset;

        public List<BuildCells> buildCells = new List<BuildCells>();

        private const int HOUSE_CODE = 1;
        private const int EMPTY_CODE = 0;
        private const int ROAD_CODE = 2;

        private void Start()
        {
            GenerateWorld();
            PrintWorldGrid();
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

            bool isHouseRow = (x % 3 == 0);
            for (int y = 0; y < sizeOfGridWorld.y; y++)
            {
                int code = DetermineCellCode(isHouseRow, y);
                buildCellsInRow.Add(new BuildCell { code = code });
            }

            return buildCellsInRow;
        }

        private int DetermineCellCode(bool isHouseRow, int y)
        {
            if (isHouseRow || y == 0 || y == sizeOfGridWorld.y - 1)
            {
                return HOUSE_CODE;
            }

            return EMPTY_CODE;
        }

        private void PlaceRoadsInRow(int rowIndex)
        {
            var maxRoadCount = CalculateMaxReplacements(buildCells[0].cells.Count);
            int roadCount = Random.Range(1, maxRoadCount);
            var availableIndexes = GetMiddleIndexes();

            for (int i = 0; i < roadCount; i++)
            {
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
            int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];

            buildCells[rowIndex].cells[randomIndex].code = ROAD_CODE;
            buildCells[rowIndex + 1].cells[randomIndex].code = ROAD_CODE;

            RemoveNearbyIndexes(availableIndexes, randomIndex);
        }

        private void RemoveNearbyIndexes(List<int> availableIndexes, int index)
        {
            availableIndexes.Remove(index);
            availableIndexes.Remove(index + 1);
            availableIndexes.Remove(index - 1);
        }

        private int CalculateMaxReplacements(int bitCount)
        {
            return (bitCount % 2 == 0) ? (bitCount / 2) : (bitCount / 2 + 1);
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
        }s
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