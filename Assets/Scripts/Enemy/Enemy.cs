using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public Animator anim;

    public bool dead=false;
    [SerializeField] private float hp=100;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void GotHit(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            hp = 0;
            dead = true;
        }
    }
}
