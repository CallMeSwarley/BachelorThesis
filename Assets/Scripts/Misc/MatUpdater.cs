using UnityEngine;

// Updates the materials when the settings are applied

public class MatUpdater : MonoBehaviour, IGlobalContextSubscriber
{
    [SerializeField] private Settings settings;
    [SerializeField] private Material voxelMat, outlineMat;

    public void update()
    {
        if (GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.main_view)
        {
            voxelMat.color = new Color(voxelMat.color.r, voxelMat.color.g, voxelMat.color.b,
                settings.getFloatByName("Voxel alpha"));
            outlineMat.color = new Color(outlineMat.color.r, outlineMat.color.g, outlineMat.color.b,
                settings.getFloatByName("Outline alpha"));
        }
    }
}