using UnityEngine;

public class SETTINGS_YES : MonoBehaviour
{
    [SerializeField] private Settings settings;

    public void OnClick()
    {
        settings.applyAll();
        GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.main_view);
    }
}