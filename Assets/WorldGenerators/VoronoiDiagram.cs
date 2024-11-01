using UnityEngine;
using UnityEngine.UI;

namespace WorldGenerators
{
    public class VoronoiDiagram : MonoBehaviour
    {
        public Vector2Int imageDim;
        public int regionAmount;
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();

            _image.sprite = Sprite.Create(GenerateVoronoiTexture(), 
                new Rect(0, 0, imageDim.x, imageDim.y),
                Vector2.one * 0.5f);
        }

        Texture2D GenerateVoronoiTexture()
        {
            Vector2Int[] centroids = new Vector2Int[regionAmount];
            Color[] regions = new Color[regionAmount];

            // Initialize centroids and region colors
            for (int i = 0; i < regionAmount; i++)
            {
                centroids[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
                regions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }

            // Prepare pixel colors for the texture
            Color[] pixelColors = new Color[imageDim.x * imageDim.y];
            for (int y = 0; y < imageDim.y; y++)
            {
                for (int x = 0; x < imageDim.x; x++)
                {
                    int index = x + y * imageDim.x;
                    pixelColors[index] = regions[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)];
                }
            }

            return GetImageFromColorArray(pixelColors);
        }

        int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
        {
            float smallestDst = float.MaxValue;
            int index = 0;
        
            for (int i = 0; i < centroids.Length; i++)
            {
                float dist = Vector2.Distance(pixelPos, centroids[i]);
                if (dist < smallestDst)
                {
                    smallestDst = dist;
                    index = i;
                }
            }
        
            return index;
        }

        Texture2D GetImageFromColorArray(Color[] pixelColors)
        {
            Texture2D tex = new Texture2D(imageDim.x, imageDim.y);
            tex.filterMode = FilterMode.Point;
            tex.SetPixels(pixelColors);
            tex.Apply();
            return tex;
        }
    }
}

/*
public class VoronoiDiagram : MonoBehaviour
{
    public int numPoints = 10; // Number of points to generate
    public int numIterations = 10; // Number of iterations for Lloyd's relaxation
    public GameObject pointPrefab; // Prefab for a point
    public GameObject linePrefab; // Prefab for a line

    private List<Vector3> points = new List<Vector3>(); // List of points
    private List<Vector3> centroids = new List<Vector3>(); // List of centroids
    public int maxWidth = 100;
    public int maxHeight = 100;

    private void Start()
    {
        GeneratePoints();
        DrawVoronoiLines();
    }

    private void GeneratePoints()
    {
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 point = new Vector3(Random.Range(0, maxWidth), 0, Random.Range(0, maxHeight));
            points.Add(point);
            Instantiate(pointPrefab, point, Quaternion.identity);
        }
    }

    private void DrawVoronoiLines()
    {
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                var line = Instantiate(linePrefab, points[i], Quaternion.identity);
                var lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, points[i]);
                lineRenderer.SetPosition(1, points[j]);
            }
        }
    }

}
*/









































/*
 public int numPoints = 10; // Number of points to generate
      public int numIterations = 10; // Number of iterations for Lloyd's relaxation
      public GameObject pointPrefab; // Prefab for a point
      public GameObject linePrefab; // Prefab for a line

      private List<Vector3> points = new List<Vector3>(); // List of points
      private List<Vector3> centroids = new List<Vector3>(); // List of centroids

      private void Start()
      {
          GeneratePoints();
          StartCoroutine(LloydsRelaxation());
      }

      private void GeneratePoints()
      {
          for (int i = 0; i < numPoints; i++)
          {
              Vector3 point = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0);
              points.Add(point);
              Instantiate(pointPrefab, point, Quaternion.identity);
          }
      }

      private IEnumerator LloydsRelaxation()
      {
          for (int i = 0; i < numIterations; i++)
          {
              centroids.Clear();

              // Calculate centroids
              for (int j = 0; j < points.Count; j++)
              {
                  Vector3 centroid = Vector3.zero;
                  int count = 0;

                  for (int k = 0; k < points.Count; k++)
                  {
                      if (j != k && Vector3.Distance(points[j], points[k]) < 100)
                      {
                          centroid += points[k];
                          count++;
                      }
                  }

                  centroid /= count;
                  centroids.Add(centroid);
              }

              // Move points to centroids
              for (int j = 0; j < points.Count; j++)
              {
                  points[j] = centroids[j];
                  Instantiate(pointPrefab, points[j], Quaternion.identity);
              }

              // Draw lines between points and centroids
              for (int j = 0; j < points.Count; j++)
              {
                  Instantiate(linePrefab, points[j], Quaternion.identity);
                  Instantiate(linePrefab, centroids[j], Quaternion.identity);
              }

              yield return new WaitForSeconds(0.5f);
          }
      }
      */