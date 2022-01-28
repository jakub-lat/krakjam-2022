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
    [SerializeField] private GameObject myHead = null;
    [SerializeField] private Canvas popupCanvas = null;
    public Canvas PopupCanvas => popupCanvas;

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
            Scoreboard.Scoreboard.Current.levelData.kills++;
            
            hp = 0;
            dead = true;
            this.gameObject.tag = "Untagged";
            myHead.tag = "Untagged";

            anim.Play("Dead");
            //GetComponent<Collider>().enabled = false;
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
