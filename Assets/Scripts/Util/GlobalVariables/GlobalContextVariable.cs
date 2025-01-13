using UnityEngine.Events;

/*
 * Stores and handles the global context variable.
 * It states in which view the user should be in at the corresponding time.
 * 
 * Scripts can subscribe to the Event that gets called whenever the value is updated.
 */
public static class GlobalContextVariable
{
    private static readonly UnityEvent VariableChange = new();

    /*
     * Views:
     *  - main_view:        The main view where the real-life marker and object of interest are visible via camera feed and, if calculated, the digital voxel volume.
     *                          Reach *photo_drawing* by taking a photo/clicking on the photo button.
     *                          Reach *gallery* by clicking on the gallery button.
     *                          Reach *settings* by clicking on the settings button.
     *  - photo_drawing:    The view when having taken a photo with the ability to draw and save or discard the shot.
     *                          Reach *main_view* by clicking on either the checkmark or x button.
     *  - gallery:          The gallery view where all saved photos are listed and can be edited and deleted and where the intersection can be triggered.
     *                          Reach *gallery_drawing* by clicking on an image and then on the edit button.
     *                          Reach *main_view* by clicking on the back button or starting the intersection.
     *  - gallery_drawing:  The view when editing the marking on a specific image from the gallery.
     *                          Reach *gallery* by clicking on either the checkmark or x button.
     *  - settings:         The view when viewing and changing the settings.
     *                          Reach *main_view* by clicking on either the checkmark or x button.
     */
    public enum GlobalContextVariableValue
    {
        main_view,
        photo_drawing,
        gallery,
        gallery_drawing,
        settings
    }

    public static GlobalContextVariableValue globalContextVariable { get; private set; } =
        GlobalContextVariableValue.main_view;

    public static GlobalContextVariableValue lastGlobalContextVariable { get; private set; } =
        GlobalContextVariableValue.main_view;

    public static void subscribe(IGlobalContextSubscriber subscriber)
    {
        VariableChange.AddListener(subscriber.update);
    }

    public static void updateValue(GlobalContextVariableValue newValue, bool forceUpdate = false)
    {
        if (forceUpdate || newValue != globalContextVariable)
        {
            lastGlobalContextVariable = globalContextVariable;
            globalContextVariable = newValue;
            VariableChange.Invoke();
        }
    }
}