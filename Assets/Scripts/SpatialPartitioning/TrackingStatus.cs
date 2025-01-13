using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TrackingStatus : MonoBehaviour
{
    [SerializeField] Image image;
    [HideInInspector] public bool targetTracked = false;
    public void displayTracked() 
    {
        if (image != null)
        {
            image.color = new Color(0.0f, 1.0f, 0.0f, 1.0f); // green
        }
        targetTracked = true;
    }

    public void displayNotTracked() 
    {
        if (image != null)
        {
            image.color = new Color(1.0f, 0.0f, 0.0f, 1.0f); // red
        }
        targetTracked = false;
    }
}
