using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    private EnemyAI ai;

    public bool dead = false;
    public bool gotHit = false;

    public float startingHealth = 100f;
    private float hp = 100;
    [SerializeField] private GameObject myHead = null;
    [SerializeField] private GameObject myBody = null;

    [SerializeField]
    private AudioSource enemySource = null;
    public AudioSource EnemySource => enemySource;

    private void Start()
    {
        //anim = GetComponentInChildren<Animator>();
        ai = GetComponent<EnemyAI>();
        hp = startingHealth;
    }

    bool firstHpChange = true;
    
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Damage = Animator.StringToHash("Damage");

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

            anim.SetTrigger(Die);

            myBody.GetComponent<Collider>().isTrigger = true;
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            return;
        }

        gotHit = true;
        anim.SetTrigger(Damage);
    }

    public void HitEnd()
    {
        gotHit = false;
    }
}
