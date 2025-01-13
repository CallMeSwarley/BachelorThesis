using UnityEngine;

public class DONE : MonoBehaviour
{
    [SerializeField] private GalleryStorage gallery;

    public void OnClick()
    {
        gallery.IntersectEverythingNew();
    }
}