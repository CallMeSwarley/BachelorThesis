using System.Collections.Generic;
using UnityEngine;

public class REDO : MonoBehaviour
{
    [SerializeField] private ScreenDrawing drawing;

    public void OnClick()
    {
        drawing.fingerLifted = false;
        drawing.points = new List<Vector3>();
        drawing.UpdateLineRenderer();
    }
}