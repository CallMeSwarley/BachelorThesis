using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Stores the currently displayed image on the camera and shows it on a UI image.
 * 
 * Displays the selected image when in gallery drawing mode.
 * 
 * Hides the UI image when exiting drawing mode (i.e., everytime a non-drawing mode is entered)
 */
public class RenderToTexture : MonoBehaviour, IGlobalContextSubscriber
{
    private static bool frozen = false;

    [SerializeField]
    private Camera cam, cam2;

    [SerializeField]
    private RawImage image;
    [SerializeField]
    private PhotoFlash photoFlash;

    [SerializeField]
    private GalleryStorage gallery;
    
    [SerializeField]
    public Text positionText;

    public Texture2D photoTexture { get; private set; }

    // This method is adapted from https://docs.unity3d.com/ScriptReference/Camera.Render.html
    private static Texture2D RTImage(Camera camera, RenderTexture renderTexture)
    {
        camera.targetTexture = renderTexture;
        
        var currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        
        camera.Render();
        
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        
        RenderTexture.active = currentRT;

        return image;
    }

    public void freezeImage(Camera camera, Camera secondaryCamera, RenderTexture renderTexture, RawImage image)
    {
        if (frozen) return;

        frozen = true;

        photoFlash?.photoFlash(0.4f);

        photoTexture = RTImage(camera, renderTexture);

        image.enabled = true;
        image.texture = photoTexture;
    }

    public void unfreezeImage(Camera camera, Camera secondaryCamera, RawImage image)
    {
        if (!frozen) return;

        frozen = false;

        image.enabled = false;
        camera.targetTexture = null;

    }

    public void update()
    {     
        switch (GlobalContextVariable.globalContextVariable)
        {
            case GlobalContextVariable.GlobalContextVariableValue.photo_drawing:
                freezeImage(cam, cam2, new RenderTexture(cam.pixelWidth, cam.pixelHeight, 32), image);
                break;
            case GlobalContextVariable.GlobalContextVariableValue.main_view:
                unfreezeImage(cam, cam2, image);
                break;
            case GlobalContextVariable.GlobalContextVariableValue.gallery_drawing:
                image.enabled = true;
                image.texture = gallery.GetActiveImage();
                break;
            case GlobalContextVariable.GlobalContextVariableValue.gallery:
                image.enabled = false;
                break;
        }
    }
}
