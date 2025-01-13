using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
 * Represents a headline in the settings menu for better readability
 */ 
public class HeadlineUI : MonoBehaviour
{
    [SerializeField] private Text text;

    public void setName(string name)
    {
        text.text = name;
        text.color = Color.white;
    }
}
