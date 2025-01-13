using System.Collections;
using System.Collections.Generic;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEngine;
using UnityEngine.UI;

// This class handles drawing input on images that is used for voxel-carving.

[RequireComponent(typeof(LineRenderer))]
public class ScreenDrawing : MonoBehaviour, IGlobalContextSubscriber
{
    [SerializeField] private bool allowMouseDrawing = true;
    [SerializeField] public Camera drawingCam;
    [SerializeField] public CamPos camPos;

    public List<Vector3> points = new(); // stored backwards to save lookup time
    public List<Vector3> worldPoints = new();

    // distanceThreshold:   The higher it is set, the longer the distance between the new point and the last added point can be.
    // angleThreshold:      The higher is is set, the larger the angle between three points can be before NOT being considered a straight line.
    //                      If a new point is added and "in line" (according to angleThreshold) with the two points added before, the middle point is simply ignored.
    // frequencyThreshold:  The higher it is set, the longer a straight line can be, before adding a point to it to split it up.
    private const float DistanceThreshold = 0.01f, AngleThreshold = 0.03f, FrequencyThreshold = 0.1f;

    // rendering depth of the drawing
    private const float ZDepth = 20;

    private LineRenderer _lineRenderer;

    public bool fingerLifted;
    private Touch _fingerNow;

    public Button yes;
    public Button redo;
    public Button no;

    private enum PointAddition
    {
        // initial state:                       O---O---O   +   *
        None, // don't add the new point              O---O---O
        Addition, // add the new point                    O---O---O-------*
        Replacement // replace the newest existing point    O---O-----------*       this might need to propagate back until no replacement
    }

    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private IEnumerator DrawingRoutineAction()
    {
        while (GlobalContextVariable.globalContextVariable ==
               GlobalContextVariable.GlobalContextVariableValue.photo_drawing ||
               GlobalContextVariable.globalContextVariable ==
               GlobalContextVariable.GlobalContextVariableValue.gallery_drawing)
        {
            Vector3 position;
            //maybe insert preprocessor directives to not check mouse input in mobile build?
            if ((allowMouseDrawing && ReceiveMouseDrawing(out position)) || ReceiveTouchDrawing(out position))
            {
                var state = EvaluateNewPoint(position);

                if (fingerLifted == false && _fingerNow.phase == TouchPhase.Moved)
                {
                    yes.gameObject.SetActive(false);
                    redo.gameObject.SetActive(false);
                    no.gameObject.SetActive(false);
                }

                // replace all points which are "in line"
                while (state == PointAddition.Replacement)
                {
                    points.RemoveAt(0);
                    state = EvaluateNewPoint(position);
                    // It might be possible, that points get removed, but in the end, not replaced, because the last evaluation results in Addition.None.
                    // As the algorithm seems to work properly in testing, we are just going to assume that this will not cause any problems.
                }

                // add the new point to the end of the current line if valid
                if (state == PointAddition.Addition)
                {
                    points.Insert(0, position);
                    UpdateLineRenderer();
                }
            }

            yield return new WaitForEndOfFrame();
            yes.gameObject.SetActive(true);
            redo.gameObject.SetActive(true);
            no.gameObject.SetActive(true);
        }
    }

    // Stores points for automatic cube adjustment in world space for later use.
    public void GenerateWorldPoints()
    {
        var minX0 = 1000.0f;
        var minY0 = 1000.0f;
        var maxX0 = -1000.0f;
        var maxY0 = -1000.0f;

        foreach (var t in points)
        {
            if (drawingCam.WorldToViewportPoint(t).x < minX0) minX0 = drawingCam.WorldToViewportPoint(t).x;
            if (drawingCam.WorldToViewportPoint(t).x > maxX0) maxX0 = drawingCam.WorldToViewportPoint(t).x;
            if (drawingCam.WorldToViewportPoint(t).y < minY0) minY0 = drawingCam.WorldToViewportPoint(t).y;
            if (drawingCam.WorldToViewportPoint(t).y > maxY0) maxY0 = drawingCam.WorldToViewportPoint(t).y;
        }

        var frustumCorners = new Vector3[4];
        camPos.mainCameraCopy.CalculateFrustumCorners(new Rect(minX0, minY0, maxX0, maxY0),
            camPos.mainCameraCopy.farClipPlane,
            Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        for (var i = 0; i < 4; i++)
        {
            var worldSpaceCorner = camPos.mainCameraCopy.transform.TransformVector(frustumCorners[i]);
            worldPoints.Add(worldSpaceCorner);
        }

        worldPoints.Add(camPos.mainCameraCopy.transform.position);
    }

    // checks if the mouse position is valid and returns its coordinates
    private bool ReceiveMouseDrawing(out Vector3 position)
    {
        if (!Input.GetMouseButton(0))
        {
            position = Vector3.zero;
            return false;
        }

        var point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ZDepth);
        position = drawingCam.ScreenToWorldPoint(point);

        return true;
    }

    // checks if the touch position is valid and returns its coordinates
    // When using an iOS device, this function is never used!!!
    private bool ReceiveTouchDrawing(out Vector3 position)
    {
        position = Vector3.zero;
        if (Input.touchCount < 1 || Input.touches[0].phase != TouchPhase.Moved) return false;

        var pos = Input.GetTouch(0).position;
        position = drawingCam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, ZDepth));

        return true;
    }

    // evaluates what is to do with a newly received point based on the existing point chain
    private PointAddition EvaluateNewPoint(Vector3 newPoint)
    {
        // add the first two points no matter what
        // because the two points before are needed in the evaluation of new points
        if (points.Count <= 1) return PointAddition.Addition;

        _fingerNow = Input.GetTouch(0);
        if (_fingerNow.phase == TouchPhase.Began || fingerLifted)
        {
            fingerLifted = true;
            return PointAddition.None;
        }

        // if the two points before are too far away (maybe because point[0] keeps being replaced by new points) then add a third point instead of replacing
        var frequencyScore = Vector3.Distance(points[0], points[1]);
        if (frequencyScore > FrequencyThreshold) return PointAddition.Addition;

        // if our three points are almost on a line, delete the middle one
        var angleScore =
            0.5f - 0.5f * Vector3.Dot((newPoint - points[0]).normalized, (points[0] - points[1]).normalized);
        if (angleScore < AngleThreshold) return PointAddition.Replacement;

        // add a new point if its far away from the last added point
        var distanceScore = Vector3.Distance(newPoint, points[0]);
        if (distanceScore > DistanceThreshold) return PointAddition.Addition;

        // if nothing applies, just do nothing with the new point
        return PointAddition.None;
    }

    public void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    public void update()
    {
        switch (GlobalContextVariable.globalContextVariable)
        {
            case GlobalContextVariable.GlobalContextVariableValue.photo_drawing:
            case GlobalContextVariable.GlobalContextVariableValue.gallery_drawing:
                StartCoroutine(DrawingRoutineAction());
                break;
            case GlobalContextVariable.GlobalContextVariableValue.main_view:
            case GlobalContextVariable.GlobalContextVariableValue.gallery:
                points = new List<Vector3>();
                worldPoints = new List<Vector3>();
                UpdateLineRenderer();
                break;
        }
    }

    // create a TriangleNetMesh from the stored point chain
    public TriangleNetMesh Triangulate()
    {
        var poly = new Polygon();
        poly.Add(new Contour(points.ConvertAll(vec3 => new Vertex(vec3.x, vec3.y))));
        return (TriangleNetMesh)poly.Triangulate();
    }

    // create a TriangleNetMesh from the given point chain
    public static TriangleNetMesh Triangulate(List<Vector3> points)
    {
        var poly = new Polygon();
        poly.Add(new Contour(points.ConvertAll(vec3 => new Vertex(vec3.x, vec3.y))));
        return (TriangleNetMesh)poly.Triangulate();
    }
}