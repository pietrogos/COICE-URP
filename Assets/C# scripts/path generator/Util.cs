using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Splines;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// Returns the positions, tangents, and upVectors for points along a spline separated by distance 'interval'. 
    /// Includes the beginning and end of the spline
    /// </summary>
    /// <param name="spline">The spline to sample</param>
    /// <param name="containerTransform">The transform of the object holding the spline</param>
    /// <param name="interval">The distance between sample points</param>
    /// <param name="positions">The output position array</param>
    /// <param name="tangents">The output tangent array</param>
    /// <param name="upVectors">The output upVector array</param>
    /// <returns>whether or not the operation succeeded</returns>
    public static bool SampleSpline(Spline spline, Transform containerTransform, float interval,
        out Vector3[] positions, out Vector3[] tangents, out Vector3[] upVectors)
    {
        // get the length of the spline in world space
        Matrix4x4 transformationMat = Matrix4x4.TRS(containerTransform.position, containerTransform.rotation, containerTransform.lossyScale);
        float splineLength = SplineUtility.CalculateLength(spline, transformationMat);

        // ensure the interval is valid
        if (interval <= 0 || interval > splineLength)
        {
            positions = null;
            tangents = null;
            upVectors = null;
            return false;
        }

        List<Vector3> positionList = new List<Vector3>();
        List<Vector3> tangentList = new List<Vector3>();
        List<Vector3> upVectorList = new List<Vector3>();

        // loop through spline, sampling at regular intervals
        float tInc = interval / splineLength;
        for (float t = 0; t <= 1f; t += tInc)
        {
            bool success = SplineUtility.Evaluate(spline, t, out float3 position, out float3 tangent, out float3 upVector);
            if (!success)
            {
                positions = null;
                tangents = null;
                upVectors = null;
                return false;
            }

            Vector3 newPosition = new Vector3(position.x, position.y, position.z);
            Vector3 newTangent = new Vector3(tangent.x, tangent.y, tangent.z);
            Vector3 newUpVector = new Vector3(upVector.x, upVector.y, upVector.z);

            positionList.Add(newPosition);
            tangentList.Add(newTangent);
            upVectorList.Add(newUpVector);

            if (t < 0.999f && t + tInc > 1f)
                t = 1f - tInc; // ensure that very end of spline gets sampled no matter what
        }

        positions = positionList.ToArray();
        tangents = tangentList.ToArray();
        upVectors = upVectorList.ToArray();
        return true;
    }
}