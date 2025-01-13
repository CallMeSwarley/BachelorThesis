using UnityEngine;

public class DELETE : MonoBehaviour
{
    [SerializeField] private GalleryStorage gallery;

    public void OnClick()
    {
        gallery.RemoveLatestImageMarking();
    }
}