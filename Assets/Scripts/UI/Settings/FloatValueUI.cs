using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
 * Represents a float value interface in the settings
 */
public class FloatValueUI : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text text;

    public float getValue()
    {
        /*
         * Some mobile devices use , in Unity's standard floating point UI textfield. However, those are parsed without the digits after the decimal point.
         * Because of that, we replace either all points with commas or vice versa. Whatever has a higher absolut value, must be the right value.
         * 
         * Hopefully, nobody uses commas in big numbers, like 1,000,000 as one million
         */
        float dotVal = float.Parse(inputField.text.Replace(',', '.'));
        float commaVal = float.Parse(inputField.text.Replace('.', ','));
        return Mathf.Abs(dotVal) > Mathf.Abs(commaVal) ? commaVal : dotVal;
    }

    public void setValue(float value)
    {
        inputField.text = "" + value;
    }

    public void setName(string name)
    {
        text.text = name;
        text.color = Color.white;
    }
}
