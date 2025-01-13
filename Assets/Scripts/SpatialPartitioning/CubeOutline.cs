using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Visualizes the initial voxel grid's dimensions
 */
public class CubeOutline : MonoBehaviour, IGlobalContextSubscriber
{
    private LineRenderer[] lines;

    [SerializeField] private Settings settings;

    private void Awake()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }

    public void update()
    {
        if(GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.main_view && GlobalContextVariable.lastGlobalContextVariable == GlobalContextVariable.GlobalContextVariableValue.settings)
        {
            bool show = settings.getBoolByName("Show Cube outline");

            
            foreach (var line in lines)
            {
                line.startColor = Color.blue;
                line.endColor = Color.blue;
                line.enabled = show;
            }

            float start_x, end_x, start_y, end_y, start_z, end_z;
            if (show)
            {
                start_x = settings.getFloatByName("X start");
                end_x = settings.getFloatByName("X end");
                start_y = settings.getFloatByName("Y start");
                end_y = settings.getFloatByName("Y end");
                start_z = settings.getFloatByName("Z start");
                end_z = settings.getFloatByName("Z end");

                int index = 0;
                lines[index].SetPosition(0, new Vector3(start_x, start_y, start_z));
                lines[index++].SetPosition(1, new Vector3(start_x, start_y, end_z));
                lines[index].SetPosition(0, new Vector3(start_x, end_y, start_z));
                lines[index++].SetPosition(1, new Vector3(start_x, end_y, end_z));
                lines[index].SetPosition(0, new Vector3(end_x, start_y, start_z));
                lines[index++].SetPosition(1, new Vector3(end_x, start_y, end_z));
                lines[index].SetPosition(0, new Vector3(end_x, end_y, start_z));
                lines[index++].SetPosition(1, new Vector3(end_x, end_y, end_z));

                lines[index].SetPosition(0, new Vector3(start_x, start_y, start_z));
                lines[index++].SetPosition(1, new Vector3(start_x, end_y, start_z));
                lines[index].SetPosition(0, new Vector3(start_x, start_y, end_z));
                lines[index++].SetPosition(1, new Vector3(start_x, end_y, end_z));
                lines[index].SetPosition(0, new Vector3(end_x, start_y, start_z));
                lines[index++].SetPosition(1, new Vector3(end_x, end_y, start_z));
                lines[index].SetPosition(0, new Vector3(end_x, start_y, end_z));
                lines[index++].SetPosition(1, new Vector3(end_x, end_y, end_z));

                lines[index].SetPosition(0, new Vector3(start_x, start_y, start_z));
                lines[index++].SetPosition(1, new Vector3(end_x, start_y, start_z));
                lines[index].SetPosition(0, new Vector3(start_x, start_y, end_z));
                lines[index++].SetPosition(1, new Vector3(end_x, start_y, end_z));
                lines[index].SetPosition(0, new Vector3(start_x, end_y, start_z));
                lines[index++].SetPosition(1, new Vector3(end_x, end_y, start_z));
                lines[index].SetPosition(0, new Vector3(start_x, end_y, end_z));
                lines[index++].SetPosition(1, new Vector3(end_x, end_y, end_z));
            }
        }
    }
}
