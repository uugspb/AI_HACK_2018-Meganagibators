using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public NPCModel model;

    [NonSerialized]
    public float DamageDealt;

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
