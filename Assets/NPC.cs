using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public NPCModel model;

    [NonSerialized]
    public float DamageDealt;
}
