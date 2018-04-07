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
        var time = (1 / model.Speed) / 5;
        if (CircleAnim)
        {
            LeanTween.rotateAround(AnimationObject, Vector3.forward, 360f, time)
            .setOnComplete(MakeAnimation);
        }
    }

    public void NearTower()
    {
        LeanTween.cancel(AnimationObject);
        isRotate = false;
    }
    
    public void Kill()
    {
        animator.SetTrigger("destroy");
        LeanTween.cancel(gameObject);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
