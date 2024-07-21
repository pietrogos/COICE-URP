using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadMeshGenerator : MonoBehaviour
{
    [SerializeField]
    private SplineContainer m_splineContainer;

    [SerializeField]
    private int m_splineIndex;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float m_timeStep = 0.01f; // Step size for sampling points on the spline

    [SerializeField]
    private float roadWidth = 2f; // Width of the road

    // UV control parameters
    [SerializeField]
    private float uvScaleX = 1f; // Scale for the X coordinate of the UV

    [SerializeField]
    private float uvScaleY = 1f; // Scale for the Y coordinate of the UV

    [SerializeField]
    private float uvOffsetX = 0f; // Offset for the X coordinate of the UV

    [SerializeField]
    private float uvOffsetY = 0f; // Offset for the Y coordinate of the UV

    // Extrusion control
    [SerializeField]
    private bool extrudeMesh = false; // Whether to extrude the mesh to make it solid

    [SerializeField]
    private float extrusionHeight = 1f; // Height of the extrusion

    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (m_splineContainer == null || m_splineIndex < 0 || m_splineIndex >= m_splineContainer.Splines.Count)
        {
            Debug.LogWarning("Invalid SplineContainer or Spline Index");
            return;
        }

        GenerateRoadMesh();
    }

    private void GenerateRoadMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int vertIndex = 0;
        float uvX = uvOffsetX; // Initialize the starting point for the X-coordinate of UVs

        Vector3 previousLeft = Vector3.zero;
        Vector3 previousRight = Vector3.zero;

        bool isClosed = m_splineContainer.Splines[m_splineIndex].Closed;

        for (float t = 0; t <= 1; t += m_timeStep)
        {
            m_splineContainer.Evaluate(m_splineIndex, t, out float3 position, out float3 tangent, out float3 upVector);

            // Calculate the left and right points, ensuring a constant width
            float3 cross = math.normalize(math.cross(tangent, upVector));
            float3 left = position - cross * (roadWidth / 2f);
            float3 right = position + cross * (roadWidth / 2f);

            if (vertIndex > 0)
            {
                // Calculate the distance between the current left point and the previous left point
                float distance = Vector3.Distance(previousLeft, left);
                uvX += distance * uvScaleX;
            }

            vertices.Add(left);
            vertices.Add(right);

            // Add UVs with updated X-coordinate and fixed Y-coordinates for the width
            uvs.Add(new Vector2(uvX, uvOffsetY));
            uvs.Add(new Vector2(uvX, uvOffsetY + uvScaleY));

            if (vertIndex > 0)
            {
                triangles.Add(vertIndex - 2);
                triangles.Add(vertIndex - 1);
                triangles.Add(vertIndex);

                triangles.Add(vertIndex - 1);
                triangles.Add(vertIndex + 1);
                triangles.Add(vertIndex);
            }

            previousLeft = left;
            previousRight = right;
            vertIndex += 2;
        }

        if (extrudeMesh)
        {
            ExtrudeMesh(vertices, triangles, uvs, isClosed);
        }

        UpdateMesh(vertices, triangles, uvs);
    }

    private void ExtrudeMesh(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, bool isClosed)
    {
        int vertCount = vertices.Count;
        List<Vector3> bottomVertices = new List<Vector3>();

        for (int i = 0; i < vertCount; i++)
        {
            Vector3 bottomVertex = vertices[i] - new Vector3(0, extrusionHeight, 0);
            bottomVertices.Add(bottomVertex);
            vertices.Add(bottomVertex);

            // Duplicate UVs for bottom vertices
            uvs.Add(uvs[i]);
        }

        // Create side faces
        for (int i = 0; i < vertCount; i += 2)
        {
            if (!isClosed && i >= vertCount - 2)
                break;

            int nextIndex = (i + 2) % vertCount;

            triangles.Add(i);
            triangles.Add(nextIndex);
            triangles.Add(i + vertCount);

            triangles.Add(nextIndex);
            triangles.Add(nextIndex + vertCount);
            triangles.Add(i + vertCount);

            triangles.Add(i + 1);
            triangles.Add(i + 1 + vertCount);
            triangles.Add(nextIndex + 1);

            triangles.Add(nextIndex + 1 + vertCount);
            triangles.Add(nextIndex + 1);
            triangles.Add(i + 1 + vertCount);
        }

        // Create bottom faces for non-closed splines
        if (!isClosed)
        {
            for (int i = 0; i < vertCount - 2; i += 2)
            {
                triangles.Add(vertCount + i);
                triangles.Add(vertCount + i + 2);
                triangles.Add(vertCount + i + 1);

                triangles.Add(vertCount + i + 1);
                triangles.Add(vertCount + i + 2);
                triangles.Add(vertCount + i + 3);
            }
        }
    }

    private void UpdateMesh(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (m_splineContainer == null || m_splineIndex < 0 || m_splineIndex >= m_splineContainer.Splines.Count)
        {
            return;
        }

        Gizmos.color = Color.red;

        for (float t = 0; t <= 1; t += m_timeStep)
        {
            m_splineContainer.Evaluate(m_splineIndex, t, out float3 position, out float3 tangent, out float3 upVector);

            // Calculate the left and right points, ensuring a constant width
            float3 cross = math.normalize(math.cross(tangent, upVector));
            float3 left = position - cross * (roadWidth / 2f);
            float3 right = position + cross * (roadWidth / 2f);

            Gizmos.DrawSphere(left, 0.1f);
            Gizmos.DrawSphere(right, 0.1f);
        }
    }
}
