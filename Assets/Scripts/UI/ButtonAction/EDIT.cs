using UnityEngine;

public class EDIT : MonoBehaviour
{
    [SerializeField] private GalleryStorage gallery;

    public void OnClick()
    {
        if (gallery.lastImageSelected != -1)
            GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.gallery_drawing);
        else
            gallery.popupMessage.PopUp(PopupMessage.NoPhotoSelected);
    }
}