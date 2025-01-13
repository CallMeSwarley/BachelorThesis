using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
 * Represents an integer value interface in the settings
 */
public class IntValueUI : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text text;

    public int getValue()
    {
        return int.Parse(inputField.text);
    }

    public void setValue(int value)
    {
        inputField.text = "" + value;
    }

    public void setName(string name)
    {
        text.text = name;
        text.color = Color.white;
    }
}
