using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Enemy))]
public abstract class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    protected Transform player;
    public LayerMask ground, playerMask, attackMask;

    public Transform lookPoint;
    public float attackRange = 5f;
    public float followRange = 30f;
    public float fleeRange = 0f;
    public float fleeMultiplier = 5f;
    public float moveSpeed = 3f;
    public float fleeSpeed = 5f;
    public bool flee = false;

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
        RaycastHit hit;
        RaycastHit hit2;
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (!Physics.Raycast(transform.position, player.transform.position, out hit2, default, attackMask) && dist > followRange) return;

        
        Physics.SphereCast(lookPoint.position, 0.1f, lookPoint.transform.forward /100, out hit, attackRange, attackMask);

        agent.speed = moveSpeed;
        if (hit.transform && playerMask == (playerMask | (1 << hit.transform.gameObject.layer)))
        {
            if(flee && dist <= fleeRange && !attacked)
            {
                agent.speed = fleeSpeed;
                RunFromPlayer();
                return;
            }
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            Attack();
        } else if(!attacked)
        {
            Chase();
        }

    }

    protected abstract void Chase();
    protected abstract void Attack();

    public void RunFromPlayer()
    {
        Vector3 runTo = transform.position + (transform.position - player.position).normalized * fleeMultiplier;

        NavMeshHit hit;

        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));

        agent.SetDestination(hit.position);
    }
}
