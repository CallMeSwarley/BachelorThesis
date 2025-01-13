using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPLOAD : MonoBehaviour
{
    [SerializeField] private VoxelGridVisualizer voxelGridVisualizer;
    [SerializeField] private PopupMessage popupMessage;

    public void OnClick()
    {
        if (voxelGridVisualizer.meshExists)
        {
            voxelGridVisualizer.uploadToserverAndVisualize();
            popupMessage.PopUp("UPLOADING...");
        }
        else
        {
            popupMessage.PopUp("You have to generate the Mesh before sending it!");
        }
    }
}
