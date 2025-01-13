using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Swaps objects of the UI
 */ 
public class ObjectSwap : MonoBehaviour, IGlobalContextSubscriber
{
    [SerializeField]
    private GameObject[] main_view_group, photo_drawing_group, gallery_group, gallery_drawing_group;

    public void update()
    {
        setActive(main_view_group, GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.main_view);
        setActive(photo_drawing_group, GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.photo_drawing || GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.gallery_drawing);
        setActive(gallery_group, GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.gallery);
    }

    private void setActive(GameObject[] group, bool active)
    {
        foreach(var obj in group)
        {
            obj.SetActive(active);
        }
    }
}
