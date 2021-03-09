using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSlider : MonoBehaviour
{
    public float red, green, blue;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    public void RedChanged() {
        red = redSlider.GetComponent<Slider>().value;
        SetColor();
    }
    public void GreenChanged()
    {
        green = greenSlider.GetComponent<Slider>().value;
        SetColor();
    }
    public void BlueChanged()
    {
        blue = blueSlider.GetComponent<Slider>().value;
        SetColor();
    }
    public void SetColor() {
        GetComponent<Renderer>().material.color = new Color(red, green, blue);
    }
}
