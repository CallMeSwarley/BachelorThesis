using UnityEngine;
using UnityEngine.UI;

// Represents a slider value interface in the settings
public class SliderValueUI : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] private Text text;
    [SerializeField] private Text textCurrent;

    public int GetValue()
    {
        return (int)slider.value;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }

    public void SetName(string nameSlider)
    {
        textCurrent.text = "Manual     Small     Medium     Large";
        textCurrent.color = Color.white;
        text.text = nameSlider;
        text.color = Color.white;
    }
}