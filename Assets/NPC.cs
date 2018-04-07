using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public NPCModel model;
    public GameObject AnimationObject;
    public bool CircleAnim;

    [NonSerialized]
    public float DamageDealt;

    private bool isRotate = true;

    private void Start()
    {         
        MakeAnimation();
    }

    private void MakeAnimation()
    {
        if (!isRotate) return;
        var time = (1 / model.Speed) ;
        if (CircleAnim)
        {
            LeanTween.rotate(AnimationObject, new Vector3(0, 0, 360), time)
            .setOnComplete(MakeAnimation);
        }
    }

    public void NearTower()
    {
        LeanTween.cancel(AnimationObject);
        isRotate = false;
    }


}
