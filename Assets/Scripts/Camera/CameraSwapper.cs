using UnityEngine;

public class CameraSwapper : MonoBehaviour, IGlobalContextSubscriber
{
    [SerializeField]
    private Camera main_cam, draw_cam, gallery_cam;

    public void update()
    {
        /*
         * Selects the current rendering camera based on the context
         * 
         * The highest depth gets rendered
         */
        switch(GlobalContextVariable.globalContextVariable)
        {
            case GlobalContextVariable.GlobalContextVariableValue.gallery:
                main_cam.depth = draw_cam.depth = 0;
                gallery_cam.depth = 1;
                break;
            case GlobalContextVariable.GlobalContextVariableValue.gallery_drawing:
                main_cam.depth = gallery_cam.depth = 0;
                draw_cam.depth = 1;
                break;
            case GlobalContextVariable.GlobalContextVariableValue.photo_drawing:
                main_cam.depth = gallery_cam.depth = 0;
                draw_cam.depth = 1;
                break;
            case GlobalContextVariable.GlobalContextVariableValue.main_view:
                draw_cam.depth = gallery_cam.depth = 0;
                main_cam.depth = 1;
                break;
        }
    }
}
