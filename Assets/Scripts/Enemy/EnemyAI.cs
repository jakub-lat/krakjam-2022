using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Enemy))]
public abstract class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
    public LayerMask ground, playerMask, attackMask;

    public Transform lookPoint;
    public float range;

    protected bool dead { get { return e.dead; } }
    protected bool attacked;

    protected Enemy e;

    protected void Start()
    {
        player = PlayerInstance.Current.transform;
        agent = GetComponent<NavMeshAgent>();
        e = GetComponent<Enemy>();
    }

    protected void Update()
    {
        transform.LookAt(player.transform.position);

        RaycastHit hit;
        Physics.SphereCast(lookPoint.position, 0.1f, lookPoint.transform.forward /100, out hit, range, attackMask);

        if (hit.transform && playerMask == (playerMask | (1 << hit.transform.gameObject.layer)))
        {
            Attack();
        } else if(!attacked)
        {
            Chase();
        }
    }

    protected abstract void Chase();
    protected abstract void Attack();
}
