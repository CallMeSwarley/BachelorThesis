using UnityEngine;

// Stores the current camera position and projection matrix (which also accounts for rotation) whenever photo_drawing mode is entered
public class CamPos : MonoBehaviour, IGlobalContextSubscriber
{
    [SerializeField] public Camera mainCamera;

    // This camera is a copy of mainCamera at the point in time at which the user last took a picture. 
    [SerializeField] public Camera mainCameraCopy;

    public Matrix4x4 ProjectionMat { get; private set; }
    public Vector3 Position { get; private set; }

    public void update()
    {
        switch (GlobalContextVariable.globalContextVariable)
        {
            case GlobalContextVariable.GlobalContextVariableValue.photo_drawing:
                Position = mainCamera.transform.position;
                ProjectionMat =
                    mainCamera.nonJitteredProjectionMatrix *
                    mainCamera.worldToCameraMatrix; // * Matrix4x4.Translate(-main_camera.transform.position);
                break;
        }
    }
}