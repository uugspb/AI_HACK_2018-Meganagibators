using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMessage : MonoBehaviour {

	public void Initialize(string text, float lifetime)
    {
        var label = GetComponent<Text>();
        label.text = text;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.5f);
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setDelay(lifetime).setOnComplete(()=> { Destroy(gameObject); });
    }
}
