using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    private EnemyAI ai;

    public bool dead=false;
    public bool gotHit = false;
    [SerializeField] private float hp=100;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ai = GetComponent<EnemyAI>();
    }

    public void GotHit(float amount)
    {
        if (dead) return;

        hp -= amount;
        ai.agent.SetDestination(transform.position);
        if (hp <= 0)
        {
            hp = 0;
            dead = true;
            anim.Play("Dead");
            return;
        }
        gotHit = true;
        anim.Play("Hit");
    }

    public void HitEnd()
    {
        gotHit = false;
    }
}
