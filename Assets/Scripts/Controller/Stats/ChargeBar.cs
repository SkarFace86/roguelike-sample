using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void SetMaxCharge(float charge)
    {
        slider.maxValue = charge;
        slider.value = charge;
    }

    public void SetCharge(float charge)
    {
        slider.value = charge;
        string c = charge.ToString("F1") + "%";
        if (charge == 100) c = 100 + "%";
        text.text = c;
    }
}
