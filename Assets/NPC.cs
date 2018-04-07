using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public NPCModel model;
    public ResourceBar HealthBar;
    public GameObject AnimationObject;
    public bool CircleAnim;

    [Header("Для разности анимашек")]
    public float timeEps = 10;

    [NonSerialized]
    public float DamageDealt;

    private bool isRotate = true;
    private float maximumHealth = 0f;

    private void Start()
    {         
        MakeAnimation();
    }
    private void Update()
    { 
        HealthBar.maxAmount = maximumHealth / 50;
        HealthBar.amount = model.HP / 50;
    }

    private void MakeAnimation()
    {
        if (!isRotate) return;
        if (CircleAnim)
        {
            var time = (1 / model.Speed) / 5;
            var deltime = UnityEngine.Random.Range(-(time / timeEps), time / timeEps);
            time += deltime;
            LeanTween.rotateAround(AnimationObject, Vector3.forward, 360f, time)
            .setOnComplete(MakeAnimation);
        }
        else
        {
            var time = (1 / model.Speed) / 10;
            var deltime = UnityEngine.Random.Range(-(time / timeEps), time / timeEps);
            time += deltime;
            LeanTween.scale(AnimationObject, new Vector3(1.05f, 0.95f), time)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(MakeAnimation2);
        }
    }

    private void MakeAnimation2()
    {
        if (!isRotate) return;
        if (CircleAnim)
        {
            var time = (1 / model.Speed) / 5;
            var deltime = UnityEngine.Random.Range(-(time / timeEps), time / timeEps);
            time += deltime;
            LeanTween.rotateAround(AnimationObject, Vector3.forward, 360f, time)
            .setOnComplete(MakeAnimation);
        }
        else
        {
            var time = (1 / model.Speed) / 10;
            var deltime = UnityEngine.Random.Range(-(time / timeEps), time / timeEps);
            time += deltime;
            LeanTween.scale(AnimationObject, new Vector3(0.95f, 1.05f), time)
                .setEase(LeanTweenType.easeInOutSine)
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
