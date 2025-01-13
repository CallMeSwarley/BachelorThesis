using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOWNLOAD : MonoBehaviour
{
    [SerializeField] private VoxelGridVisualizer voxelGridVisualizer;
    [SerializeField] private PopupMessage popupMessage;

    public void OnClick()
    {
        voxelGridVisualizer.downloadAndVisualizeGLTF();
        popupMessage.PopUp("DOWNLOADING");
    }
}
