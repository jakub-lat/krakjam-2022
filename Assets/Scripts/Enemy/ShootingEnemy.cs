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
    public float bulletDamage = 5f;
    public int magazineSize = 6;
    public float reloadSpeed = 3f;
    public float dispersion = 10f;
    public bool magazine=true;

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
        if (dead || !isEnabled) return;

        if(e.gotHit)
        {
            if (reloading) reloadTimer = reloadSpeed;
            return;
        }

        if (currMagazine <= 0 && magazine)
        {
            reloading = true;
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                currMagazine = magazineSize;
                reloadTimer = reloadSpeed;
                reloading = false;
            } else if (reloadTimer == reloadSpeed)
            {
                //play anim
            }

            return;
        }

        base.Update();
        
        lookPoint.transform.LookAt(player.transform.position);
    }

    protected override void Chase()
    {
        if (reloading || dead) return;
        base.Chase();
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

        Vector3 dir = (player.position - lookPoint.position).normalized * bulletSpeed;
        Vector3 dispDir = new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion));

        obj.transform.forward = dir + dispDir;
        obj.GetComponent<Rigidbody>().AddForce(dir+dispDir);
        obj.GetComponent<EnemyBullet>().damage = bulletDamage;
        if(magazine) currMagazine--;
    }
}
