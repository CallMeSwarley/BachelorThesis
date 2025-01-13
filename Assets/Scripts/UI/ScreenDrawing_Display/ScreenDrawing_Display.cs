using UnityEngine;

// Displays a screen drawing / marking

public class ScreenDrawing_Display : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    public void ShowPoints(Vector3[] points)
    {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}