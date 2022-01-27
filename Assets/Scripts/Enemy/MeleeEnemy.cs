using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI
{
    public float attackSpeed = 0.5f;
    public float attackDelay = 0.1f;
    public float knockback = 1f;
    public float damage = 10f;

    public MeleeAttack meleeEnemyScript;

    private new void Start()
    {
        base.Start();
        meleeEnemyScript.knockback = knockback;
        meleeEnemyScript.damage = damage;
    }

    private new void Update()
    {
        if (dead || e.gotHit) return;

        Debug.Log("cam");
        base.Update();

    }

    protected override void Chase()
    {
        Debug.Log("mov");
        agent.SetDestination(player.position);
    }

    protected override void Attack() 
    {
        agent.SetDestination(transform.position);

        if (!attacked)
        {
            attacked = true;

            Invoke(nameof(Hit), attackDelay);

            Invoke(nameof(ResetAttack), attackSpeed + attackDelay);
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }
    private void Hit()
    {
        // play attack animation
        e.anim.Play("Attack");
        meleeEnemyScript.attacking = true;
    }

    public void EndAttack()
    {
        meleeEnemyScript.attacking = false;
    }
}
