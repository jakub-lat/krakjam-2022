using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Enemy e;
    private MeleeEnemy melee;

    private void Start()
    {
        e = GetComponentInParent<Enemy>();
        melee = GetComponentInParent<MeleeEnemy>();
    }
    public void Kaput()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void EndHit()
    {
        e.HitEnd();
    }

    public void EndThrow()
    {
        if (melee) melee.EndAttack();
    }
}
