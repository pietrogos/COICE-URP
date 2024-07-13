using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PathCreator : MonoBehaviour
{
    public List<Vector3> pathPoints = new List<Vector3>();
    public Terrain terrain;
    public float roadWidth = 3f;
    public float smoothRadius = 5f;
    public float smoothStrength = 0.5f;
    public float heightOffset = 0f; // Height offset setting
    public int subdivisions = 5; // Number of subdivisions setting
    public int smoothingFactor = 10; // Number of points to interpolate between each pair of points

    private Mesh roadMesh;
    private GameObject roadObject;

    public void AddPoint(Vector3 point)
    {
        point.y += heightOffset; // Apply height offset
        pathPoints.Add(point);
        AdjustToTerrain();
        UpdatePath();
    }

    public void ClearPoints()
    {
        pathPoints.Clear();
    }

    public void GeneratePath()
    {
        roadMesh = GenerateMesh();
        if (roadObject == null)
        {
            roadObject = new GameObject("Road");
            roadObject.AddComponent<MeshFilter>().mesh = roadMesh;
            roadObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        }
        else
        {
            roadObject.GetComponent<MeshFilter>().mesh = roadMesh;
        }
    }

    private void AdjustToTerrain()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 point = pathPoints[i];
            point.y = terrain.SampleHeight(point) + heightOffset; // Adjust to terrain and apply height offset
            pathPoints[i] = point;
        }
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Vector3> smoothedPoints = GetSmoothedPoints();

        for (int i = 0; i < smoothedPoints.Count - 1; i++)
        {
            Vector3 startPoint = smoothedPoints[i];
            Vector3 endPoint = smoothedPoints[i + 1];
            Vector3 direction = (endPoint - startPoint).normalized;
            float segmentLength = Vector3.Distance(startPoint, endPoint) / (subdivisions + 1);

            for (int j = 0; j <= subdivisions + 1; j++)
            {
                Vector3 currentPoint = startPoint + direction * segmentLength * j;

                Vector3 left = currentPoint + Vector3.left * roadWidth * 0.5f;
                Vector3 right = currentPoint + Vector3.right * roadWidth * 0.5f;

                vertices.Add(left);
                vertices.Add(right);

                if (j > 0)
                {
                    int startIndex = vertices.Count - 4;
                    triangles.Add(startIndex);
                    triangles.Add(startIndex + 2);
                    triangles.Add(startIndex + 1);
                    triangles.Add(startIndex + 1);
                    triangles.Add(startIndex + 2);
                    triangles.Add(startIndex + 3);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    public void SmoothTerrain()
    {
        if (roadMesh == null)
        {
            Debug.LogWarning("No road mesh found. Generate the path first.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        foreach (Vector3 vertex in roadMesh.vertices)
        {
            Vector3 worldVertex = roadObject.transform.TransformPoint(vertex);
            int x = (int)((worldVertex.x / terrainData.size.x) * heightmapWidth);
            int z = (int)((worldVertex.z / terrainData.size.z) * heightmapHeight);

            for (int i = -Mathf.CeilToInt(smoothRadius); i <= Mathf.CeilToInt(smoothRadius); i++)
            {
                for (int j = -Mathf.CeilToInt(smoothRadius); j <= Mathf.CeilToInt(smoothRadius); j++)
                {
                    int nx = Mathf.Clamp(x + i, 0, heightmapWidth - 1);
                    int nz = Mathf.Clamp(z + j, 0, heightmapHeight - 1);

                    float distance = Vector2.Distance(new Vector2(x, z), new Vector2(nx, nz));
                    if (distance <= smoothRadius)
                    {
                        heights[nx, nz] = Mathf.Lerp(heights[nx, nz], worldVertex.y / terrainData.size.y, smoothStrength);
                    }
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Gizmos.DrawSphere(pathPoints[i], 0.5f); // Draw spheres at path points
            if (i < pathPoints.Count - 1)
            {
                Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]); // Draw lines between points
            }
        }
    }

    public void UpdatePath()
    {
        if (roadObject != null)
        {
            AdjustToTerrain();
            roadMesh = GenerateMesh();
            roadObject.GetComponent<MeshFilter>().mesh = roadMesh;
        }
    }

    private List<Vector3> GetSmoothedPoints()
    {
        List<Vector3> smoothedPoints = new List<Vector3>();

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? pathPoints[i] : pathPoints[i - 1];
            Vector3 p1 = pathPoints[i];
            Vector3 p2 = pathPoints[i + 1];
            Vector3 p3 = i == pathPoints.Count - 2 ? pathPoints[i + 1] : pathPoints[i + 2];

            for (int j = 0; j < smoothingFactor; j++)
            {
                float t = j / (float)smoothingFactor;
                Vector3 point = CatmullRom(p0, p1, p2, p3, t);
                smoothedPoints.Add(point);
            }
        }

        smoothedPoints.Add(pathPoints[pathPoints.Count - 1]); // Add the last point

        return smoothedPoints;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float f0 = -0.5f * t3 + t2 - 0.5f * t;
        float f1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
        float f2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
        float f3 = 0.5f * t3 - 0.5f * t2;

        return p0 * f0 + p1 * f1 + p2 * f2 + p3 * f3;
    }
}