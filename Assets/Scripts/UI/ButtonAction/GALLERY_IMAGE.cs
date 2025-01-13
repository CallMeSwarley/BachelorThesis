using UnityEngine;

public class GALLERY_IMAGE : MonoBehaviour
{
    public int index;
    public GalleryStorage gallery;

    public void OnClick()
    {
        gallery.ShowImage(index);
    }
}