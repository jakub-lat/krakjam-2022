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
        meleeEnemyScript.myPos = transform;
    }

    private new void Update()
    {
        if (dead || e.gotHit || !isEnabled) return;

        base.Update();

    }

    protected override void Chase()
    {
        //if (Vector3.Distance(transform.position, player.transform.position) < attackRange) return;

        agent.SetDestination(player.position + (transform.position-player.position).normalized*attackRange);
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
