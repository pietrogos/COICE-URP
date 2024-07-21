using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineSampler : MonoBehaviour
{
    [SerializeField]
    private SplineContainer m_splineContainer;

    [SerializeField]
    private int m_splineIndex;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_time;

    float3 position;
    float3 tangent;
    float3 upVector;

    private void Update()
    {
        // Check if m_splineContainer is assigned
        if (m_splineContainer == null)
        {
            Debug.LogWarning("SplineContainer is not assigned.");
            return;
        }

        // Check if m_splineIndex is within valid range
        if (m_splineIndex < 0 || m_splineIndex >= m_splineContainer.Splines.Count)
        {
            Debug.LogWarning("Spline index is out of range.");
            return;
        }

        // Evaluate the spline at the given time
        m_splineContainer.Evaluate(m_splineIndex, m_time, out position, out tangent, out upVector);

        // Log the evaluated position
        Debug.Log($"Evaluated Position: {position}, Tangent: {tangent}, UpVector: {upVector}");
    }

 
}
