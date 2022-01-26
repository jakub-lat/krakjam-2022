using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
    public LayerMask ground, playerMask, attackMask;

    public Transform lookPoint;
    public float range;

    protected bool dead = false;
    protected bool attacked;

    protected void Start()
    {
        player = PlayerInstance.instance.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        transform.LookAt(player.transform.position);

        RaycastHit hit;
        Physics.SphereCast(lookPoint.position, 0.1f, lookPoint.transform.forward /100, out hit, range, attackMask);

        if (hit.transform && hit.transform.gameObject.layer==6)
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
