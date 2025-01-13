using UnityEngine;

public class SD_NO : MonoBehaviour
{
    [SerializeField] private ScreenDrawing drawing;

    public void OnClick()
    {
        if (GlobalContextVariable.globalContextVariable ==
            GlobalContextVariable.GlobalContextVariableValue.gallery_drawing)
            GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.gallery);
        else if (GlobalContextVariable.globalContextVariable ==
                 GlobalContextVariable.GlobalContextVariableValue.photo_drawing ||
                 GlobalContextVariable.globalContextVariable ==
                 GlobalContextVariable.GlobalContextVariableValue.settings)
            GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.main_view);
        
        if(drawing != null)
            drawing.fingerLifted = false;
    }
}