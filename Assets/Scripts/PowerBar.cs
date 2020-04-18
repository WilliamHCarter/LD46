using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Slider slider;

    public void SetValue(int val)
    {
        slider.value = val;
    }
    public void setMaxValue(int max)
    {
        slider.maxValue = max;
        slider.value = 0;
    }
}
