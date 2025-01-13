using System.Threading.Tasks;
using UnityEngine;

public class GALLERY : MonoBehaviour
{
    [SerializeField] private GalleryStorage gallery;
    [SerializeField] private PopupMessage popupMessage;
    [SerializeField] private VoxelGridVisualizer visualizer;

    public void OnClick()
    {
        //TODO change back after all ist done
        //gallery.IntersectEverythingNew();

        //visualizer.visualizeImported();
        //if (gallery.GetImageMarkingCount() > 2)
        //    GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.gallery);
        //else
        //{
        //    popupMessage.PopUp("Not enough photos taken. Using default");
        //gallery.IntersectEverythingNew();
        //}

        GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.gallery);
        if (gallery.GetImageMarkingCount() > 0)
            GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.gallery);
        else
            popupMessage.PopUp(PopupMessage.NotEnoughPhotos);
    }
}