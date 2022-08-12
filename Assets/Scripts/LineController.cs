using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField]
    private float posY = 1;
    private LineRenderer lineRenderer;
    [HideInInspector]
    public List<Vector3> points;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        points = new List<Vector3>();
    }

    private void LateUpdate()
    {
        if (points.Count >= 2)
        {
            for (int i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }

    public void AddPoint(Vector3 point)
    {
        lineRenderer.positionCount++;
        points.Add(new Vector3(point.x, posY, point.z));
    }
}
