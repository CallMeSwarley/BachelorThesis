using UnityEngine;

public class SD_YES : MonoBehaviour
{
    [SerializeField] private SP_Controller spController;
    [SerializeField] private ScreenDrawing drawingImage;
    [SerializeField] private GalleryStorage gallery;

    public void OnClick()
    {
        if (GlobalContextVariable.globalContextVariable ==
            GlobalContextVariable.GlobalContextVariableValue.photo_drawing)
        {
            spController.SaveImageMarking();
            drawingImage.GenerateWorldPoints();
            GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.main_view);
        }
        else if (GlobalContextVariable.globalContextVariable ==
                 GlobalContextVariable.GlobalContextVariableValue.gallery_drawing)
        {
            spController.UpdateImageMarking();
            drawingImage.camPos.mainCameraCopy.transform.position.Set(
                gallery.imageMarkings[gallery.lastImageSelected].Position.x,
                gallery.imageMarkings[gallery.lastImageSelected].Position.y,
                gallery.imageMarkings[gallery.lastImageSelected].Position.z);
            drawingImage.camPos.mainCameraCopy.transform.rotation.Set(
                gallery.imageMarkings[gallery.lastImageSelected].Rotation.w,
                gallery.imageMarkings[gallery.lastImageSelected].Rotation.x,
                gallery.imageMarkings[gallery.lastImageSelected].Rotation.y,
                gallery.imageMarkings[gallery.lastImageSelected].Rotation.z);
            drawingImage.GenerateWorldPoints();
            GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.gallery);
        }

        drawingImage.fingerLifted = false;
    }
}