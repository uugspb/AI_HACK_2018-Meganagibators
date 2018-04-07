using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ParameterSlider : MonoBehaviour {

    public event Action<ParameterSlider> OnAddButton;
    public Text label;

    public float value
    {
        get { return float.Parse(label.text); }
        set { label.text = value.ToString(); }
    } 

    public void AddButtonClick()
    {
        if (OnAddButton != null)
            OnAddButton(this);
    }

}
