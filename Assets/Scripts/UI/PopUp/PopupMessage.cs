using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Handles user warnings by creating pop-up messages

public class PopupMessage : MonoBehaviour
{
    [SerializeField] private MaskableGraphic[] renderers;

    [SerializeField] private Text textObj;

    public static readonly string
        NotEnoughPhotos = "Not enough photos have been taken!",
        NoPhotoSelected = "No photos have been selected!",
        NoMarking = "No markings have been added!",
        SameAngle = "The photos taken do not show the object of interest from different angles!";

    public void PopUp(string messageText, float seconds = 1)
    {
        StopAllCoroutines();
        StartCoroutine(Show(messageText, seconds));
    }

    private IEnumerator Show(string messageText, float seconds)
    {
        textObj.text = messageText;

        foreach (var rendererL in renderers)
            rendererL.color = new Color(rendererL.color.r, rendererL.color.g, rendererL.color.b, 1);

        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        seconds = 1;
        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            foreach (var rendererL in renderers)
                rendererL.color = new Color(rendererL.color.r, rendererL.color.g, rendererL.color.b, seconds);
            yield return new WaitForEndOfFrame();
        }

        foreach (var rendererL in renderers)
            rendererL.color = new Color(rendererL.color.r, rendererL.color.g, rendererL.color.b, 0);
    }
}