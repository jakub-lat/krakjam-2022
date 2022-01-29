using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    private EnemyAI ai;

    public bool dead=false;
    public bool gotHit = false;

    public float startingHealth = 100f;
    private float hp=100;
    [SerializeField] private GameObject myHead = null;
    [SerializeField] private GameObject myBody = null;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ai = GetComponent<EnemyAI>();
        hp = startingHealth;
    }

    bool firstHpChange = true;
    public void HPrefresh()
    {
        if (!firstHpChange) return;
        firstHpChange = true;
        hp = startingHealth;
    }

    public void GotHit(float amount)
    {
        if (dead) return;

        hp -= amount;
        ai.agent.SetDestination(transform.position);
        if (hp <= 0)
        {
            Scoreboard.GameScoreboard.Current.levelData.kills++;
            LevelManager.Current.Score += LevelManager.Current.killScore;
            
            hp = 0;
            dead = true;
            myHead.tag = "Untagged";
            myBody.tag = "Untagged";

            anim.Play("Dead");

            myBody.GetComponent<Collider>().isTrigger = true;
            foreach(Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

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
