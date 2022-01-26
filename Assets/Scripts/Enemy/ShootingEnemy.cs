using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : EnemyAI
{
    [Header("Shooting")]
    public string bulletPoolTag;

    public float attackSpeed = 0.5f;
    public float shootDelay = 0.1f;
    public float bulletSpeed = 32f;
    public int magazineSize = 6;
    public float reloadSpeed = 3f;

    private int currMagazine;
    private float reloadTimer;

    private new void Start()
    {
        base.Start();

        currMagazine = magazineSize;
        reloadTimer = reloadSpeed;
    }

    private bool reloading = false;
    private new void Update()
    {
        if (dead) return;

        if (currMagazine <= 0)
        {
            reloading = true;
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                currMagazine = magazineSize;
                reloadTimer = reloadSpeed;
                reloading = false;
            }

            return;
        }

        base.Update();
        
        lookPoint.transform.LookAt(player.transform.position);
    }

    protected override void Chase()
    {
        if (reloading || dead) return;
        agent.SetDestination(player.position);
    }

    protected override void Attack()
    {
        if (reloading || dead) return;
        agent.SetDestination(transform.position);

        if (!attacked)
        {
            attacked = true;

            Invoke(nameof(Shoot), shootDelay);

            Invoke(nameof(ResetAttack), attackSpeed + shootDelay);
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }
    private void Shoot()
    {
        GameObject obj = ObjectPooler.Current.SpawnPool(bulletPoolTag, lookPoint.position, Quaternion.identity);
        obj.GetComponent<Rigidbody>().AddForce((player.position - lookPoint.position).normalized * bulletSpeed);
        currMagazine--;
    }
}
