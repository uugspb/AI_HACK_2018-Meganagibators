using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour
{

    public SpriteRenderer background;
    public SpriteRenderer foreground;

    public float amount
    {
        set
        {
            foreground.size = new Vector2(value, foreground.size.y);
            foreground.transform.localPosition = new Vector3(-((background.size.x - value) / 2), foreground.transform.localPosition.y, foreground.transform.localPosition.z);
        }
    }

    public float maxAmount
    {
        set
        {
            background.size = new Vector2(value, background.size.y);
        }
    }

}
