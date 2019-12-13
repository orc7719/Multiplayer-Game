using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class RopeGenerator : MonoBehaviour
{
    [SerializeField] Transform ropeStart;
    [SerializeField] Transform ropeEnd;

    [SerializeField] GameObject light01;
    [SerializeField] GameObject light02;

    LineRenderer lineRenderer;

    List<Vector3> ropePoints = new List<Vector3>();
    List<GameObject> lights = new List<GameObject>();

    [ContextMenu("Update Rope")]
    void UpdateRope()
    {
        lineRenderer = GetComponent<LineRenderer>();

        Vector3 A = ropeStart.position;
        Vector3 D = ropeEnd.position;

        //Add small curve at start and end of hose
        Vector3 B = A + ropeStart.up * (-(A - D).magnitude * 0.1f);
        Vector3 C = D + ropeEnd.up * ((A - D).magnitude * 0.5f);

        BezierCurve.GetBezierCurve(A, B, C, D, ropePoints);

        Vector3[] positions = new Vector3[ropePoints.Count];

        for (int i = 0; i < ropePoints.Count; i++)
        {
            positions[i] = ropePoints[i];
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    [ContextMenu("Add Lights")]
    void AddLights()
    {
        for (int i = 0; i < lights.Count-2; i++)
        {
            DestroyImmediate(lights[i]);
        }
        lights.Clear();

        Debug.Log("Rope Points: "+ropePoints.Count);

        bool oddLight = false;

        for (int i = 1; i < ropePoints.Count; i+=2)
        {
            GameObject lightType = oddLight ? light01 : light02;
            oddLight = !oddLight;

            GameObject newLight = Instantiate(lightType, ropePoints[i], Quaternion.identity, transform);
            lights.Add(newLight);
            Debug.Log(i);
        }
    }
}