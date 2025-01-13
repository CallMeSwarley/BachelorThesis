using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Handles the UI and data for the settings menu.
 * Any script trying to get a user defined settings value, calls this script.
 */
public class Settings : MonoBehaviour, IGlobalContextSubscriber
{

    [SerializeField] internal GameObject headlineUI, boolValueUI, intValueUI, floatValueUI, sliderValueUI, containerGeneral, containerExpert, containerParent, settings;

    private List<SettingsValue> settingsValues;
    const int generalSettingsCount = 4; // the first x settings are always displayed (Headline counts as setting)
    bool displayingExpert;

    public void update()
    {
        if(GlobalContextVariable.globalContextVariable == GlobalContextVariable.GlobalContextVariableValue.settings)
        {
            settings.SetActive(true);
            foreach(var settingsValue in settingsValues)
            {
                settingsValue.openSettings();
            }
        } else
        {
            HideExpert();
            settings.SetActive(false);
        }
    }
    public void ExpertButton()
    {
        if (displayingExpert)
        {
            HideExpert();
        }
        else
        {
            ShowExpert();
        }
    }

    void ShowExpert()
    {
        Debug.Log("Display Expert!");
        containerExpert.SetActive(true);
        displayingExpert = true;
    }

    void HideExpert()
    {
        Debug.Log("Hide Expert!");
        containerExpert.SetActive(false);
        displayingExpert = false;
    }

    #region getters and setters
    public float getFloatByName(string name)
    {
        foreach(var settingsValue in settingsValues)
        {
            if(typeof(FloatValue).IsAssignableFrom(settingsValue.GetType()) && settingsValue.name.Equals(name))
            {
                return ((FloatValue)settingsValue).actualValue;
            }
        }
        throw new KeyNotFoundException("There is not float value with name " + name + "!");
    }
    
    // I know this is bad, but I needed to get it working quickly.
    public void setXStart(float f)
    {
        ((FloatValue)settingsValues[8]).actualValue = f;
    }
    
    public void setXEnd(float f)
    {
        ((FloatValue)settingsValues[9]).actualValue = f;
    }
    
    public void setYStart(float f)
    {
        ((FloatValue)settingsValues[10]).actualValue = f;
    }
    
    public void setYEnd(float f)
    {
        ((FloatValue)settingsValues[11]).actualValue = f;
    }
    
    public void setZStart(float f)
    {
        ((FloatValue)settingsValues[12]).actualValue = f;
    }
    
    public void setZEnd(float f)
    {
        ((FloatValue)settingsValues[13]).actualValue = f;
    }

    public void setInitialVoxelSize(float f)
    {
        ((FloatValue)settingsValues[14]).actualValue = f;
    }

    public bool getBoolByName(string name)
    {
        foreach (var settingsValue in settingsValues)
        {
            if (typeof(BoolValue).IsAssignableFrom(settingsValue.GetType()) && settingsValue.name.Equals(name))
            {
                return ((BoolValue)settingsValue).actualValue;
            }
        }
        throw new KeyNotFoundException("There is not bool value with name " + name + "!");
    }

    public int getSliderByName(string name)
    {
        foreach (var settingsValue in settingsValues)
            if (typeof(SliderValue).IsAssignableFrom(settingsValue.GetType()) && settingsValue.name.Equals(name))
                return ((SliderValue)settingsValue).actualValue;
        throw new KeyNotFoundException("There is not slider value with name " + name + "!");
    }
    
    public int getIntByName(string name)
    {
        foreach (var settingsValue in settingsValues)
        {
            if (typeof(IntValue).IsAssignableFrom(settingsValue.GetType()) && settingsValue.name.Equals(name))
            {
                return ((IntValue)settingsValue).actualValue;
            }
        }
        throw new KeyNotFoundException("There is not int value with name " + name + "!");
    }
    #endregion getter

    private void Awake()
    {
        // New settings parameters are just added by type, name and initial value.
        // The settings class takes care of the UI then.
        // External scripts can get the current value by getting them via the name string.
        settingsValues = new List<SettingsValue>() {
            new Headline(headlineUI, "General Adjustments"),
            new BoolValue(boolValueUI, "MarchingCubes method", true),
            new IntValue(intValueUI, "Initial Voxel count", 100),
            new IntValue(intValueUI, "Max number subdivision iterations", 3),

            new Headline(headlineUI, "Expert Adjustments"),
            // Instead of cube being initialized as small cube on top of marker, the cube is adjusted so it contains the object of interest.
            new BoolValue(boolValueUI, "Smart Cube Adjustment", true),
            new BoolValue(boolValueUI, "Show Cube outline", false),
            new FloatValue(floatValueUI, "Outline alpha", 0.8f),
            new FloatValue(floatValueUI, "X start", -0.1f),
            new FloatValue(floatValueUI, "X end", 0.1f),
            new FloatValue(floatValueUI, "Y start", 0),
            new FloatValue(floatValueUI, "Y end", 0.2f),
            new FloatValue(floatValueUI, "Z start", -0.1f),
            new FloatValue(floatValueUI, "Z end", 0.1f),
            //new SliderValue(sliderValueUI, "Object Size", 1),
            new FloatValue(floatValueUI, "Initial voxel size", 0.5f),
            new FloatValue(floatValueUI, "Voxel alpha", 0.35f),            
        };

        // set the UI up
        float offset = 0;
        for (int i = 0; i < generalSettingsCount; i++)
        {
            offset = settingsValues[i].initialize(containerGeneral, offset);
        }
        for (int i = generalSettingsCount; i < settingsValues.Count; i++)
        {
            offset = settingsValues[i].initialize(containerExpert, offset);
        }
        RectTransform contentPanel = containerParent.GetComponent<RectTransform>();
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, offset);

        HideExpert();
    }

    // apply the values set in the UI to all internal values
    public void applyAll()
    {
        foreach (var settingsValue in settingsValues)
        {
            settingsValue.applyValue();
        }
    }

    /*
     * The value elements, which handle UI and value storage
     */
    #region settingsValues
    internal abstract class SettingsValue
    {
        internal string name { get; set; }
        public GameObject valueUI;

        internal SettingsValue(GameObject valueUI, string name)
        {
            this.valueUI = valueUI;
            this.name = name;
        }

        private bool wasInitialized = false;

        internal float initialize(GameObject container, float offset)
        {
            float ret = 0;
            if(!wasInitialized)
            {
                wasInitialized = true;
                ret = _initialize(container, offset);
            }
            openSettings();
            return ret;
        }

        internal abstract void openSettings();

        protected abstract float _initialize(GameObject container, float offset);

        internal abstract void applyValue();
    }

    internal class BoolValue : SettingsValue
    {
        internal BoolValue(GameObject valueUI, string name, bool initialValue = false) : base(valueUI, name) { actualValue = initialValue; }

        internal bool actualValue { get; private set; }

        private BoolValueUI wrapper;

        protected override float _initialize(GameObject container, float offset)
        {
            wrapper = Instantiate(valueUI, container.transform).GetComponent<BoolValueUI>();
            wrapper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -offset);
            wrapper.setName(name);
            return offset + wrapper.GetComponent<RectTransform>().rect.height;
        }

        internal override void openSettings()
        {
            wrapper.setValue(actualValue);
        }

        internal override void applyValue()
        {
            try
            {
                actualValue = wrapper.getValue();
            }
            catch (FormatException) { }
        }
    }

    internal class SliderValue : SettingsValue
    {
        internal SliderValue(GameObject valueUI, string name, int initialValue = 1) : base(valueUI, name)
        {
            actualValue = initialValue;
        }

        internal int actualValue { get; private set; }

        private SliderValueUI wrapper;

        protected override float _initialize(GameObject container, float offset)
        {
            wrapper = Instantiate(valueUI, container.transform).GetComponent<SliderValueUI>();
            wrapper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -offset);
            wrapper.SetName(name);
            wrapper.slider.minValue = 0;
            wrapper.slider.maxValue = 3;
            return offset + wrapper.GetComponent<RectTransform>().rect.height;
        }

        internal override void openSettings()
        {
            wrapper.SetValue(actualValue);
        }

        internal override void applyValue()
        {
            try
            {
                actualValue = wrapper.GetValue();
            }
            catch (FormatException)
            {
            }
        }
    }

    internal class FloatValue : SettingsValue
    {
        internal FloatValue(GameObject valueUI, string name, float initialValue = 0) : base(valueUI, name) { actualValue = initialValue; }

        internal float actualValue { get; set; }

        private FloatValueUI wrapper;

        protected override float _initialize(GameObject container, float offset)
        {
            wrapper = Instantiate(valueUI, container.transform).GetComponent<FloatValueUI>();
            wrapper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -offset);
            wrapper.setName(name);
            return offset + wrapper.GetComponent<RectTransform>().rect.height;
        }

        internal override void openSettings()
        {
            wrapper.setValue(actualValue);
        }

        internal override void applyValue()
        {
            try
            {
                actualValue = wrapper.getValue();
            }
            catch (FormatException) { }
        }
    }

    internal class IntValue : SettingsValue
    {
        internal IntValue(GameObject valueUI, string name, int initialValue=0) : base(valueUI, name) { actualValue = initialValue;  }

        internal int actualValue { get; private set; }

        private IntValueUI wrapper;

        protected override float _initialize(GameObject container, float offset)
        {
            wrapper = Instantiate(valueUI, container.transform).GetComponent<IntValueUI>();
            wrapper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -offset);
            wrapper.setName(name);
            return offset + wrapper.GetComponent<RectTransform>().rect.height;
        }

        internal override void openSettings()
        {
            wrapper.setValue(actualValue);
        }

        internal override void applyValue()
        {
            try
            {
                actualValue = wrapper.getValue();
            }
            catch (FormatException) { }
        }
    }

    internal class Headline : SettingsValue
    {
        internal Headline(GameObject valueUI, string name) : base(valueUI, name) { }

        private HeadlineUI wrapper;

        protected override float _initialize(GameObject container, float offset)
        {
            wrapper = Instantiate(valueUI, container.transform).GetComponent<HeadlineUI>();
            wrapper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -offset);
            wrapper.setName(name);
            return offset + wrapper.GetComponent<RectTransform>().rect.height;
        }

        internal override void openSettings()
        {
            // do nothing, as this is just a headline and has no value
        }

        internal override void applyValue()
        {
            // do nothing, as this is just a headline and has no value
        }
    }
    #endregion settingsValues
}
