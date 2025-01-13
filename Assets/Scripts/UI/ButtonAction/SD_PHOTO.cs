using UnityEngine;

public class SD_PHOTO : MonoBehaviour
{
    [SerializeField] private SP_Controller spController;
    [SerializeField] TrackingStatus trackingStatus;
    public void OnClick()
    {
        if (trackingStatus != null && !trackingStatus.targetTracked)
        {
            spController.popupMessage.PopUp("No Marker is tracked!");
            return;
        }
        GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.photo_drawing);
        spController.points = spController.CreateCubeIntersectionFrustum();
    }
}