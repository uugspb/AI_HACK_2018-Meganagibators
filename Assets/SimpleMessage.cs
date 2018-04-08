using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMessage : MonoBehaviour
{
    public Action Click;
	public void Initialize(string text, float lifetime, Action onClick = null)
    {
        var label = GetComponent<Text>();
        label.text = text;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.5f);
        if (onClick == null)
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setDelay(lifetime).setOnComplete(() => { Destroy(gameObject); });
        }
        else
        {
            Click = onClick;
        }
    }

    public void OnClick()
    {
        if(Click!=null)
        {
            Click();
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() => { Destroy(gameObject); });
        }
    }
}
