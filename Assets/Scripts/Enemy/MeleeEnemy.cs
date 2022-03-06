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

    public float chaseOffset = 0.2f;

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
        base.Chase();

        var change = (transform.position - player.position) * chaseOffset;
        change.y = 0;

        agent.SetDestination(player.position + change);

        //Debug.Log("c");
    }

    protected override void Attack() 
    {
        agent.SetDestination(transform.position);
        //Debug.Log("a");

        if (!attacked)
        {
            e.anim.SetTrigger("Throw");
            attacked = true;

            Invoke(nameof(Hit), attackDelay);

            Invoke(nameof(ResetAttack), attackSpeed);
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }
    private void Hit()
    {
        // play attack animation
        meleeEnemyScript.attacking = true;
    }

    public void EndAttack()
    {
        meleeEnemyScript.attacking = false;
    }
}
