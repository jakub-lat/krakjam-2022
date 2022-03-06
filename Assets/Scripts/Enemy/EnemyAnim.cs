using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Enemy[] e;
    private MeleeEnemy melee;

    private void Start()
    {
        e = GetComponentsInParent<Enemy>();
        melee = GetComponentInParent<MeleeEnemy>();
    }
    public void Kaput()
    {
        transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void EndHit()
    {
        foreach(Enemy ce in e)
        {
            ce.HitEnd();
        }
    }

    public void EndThrow()
    {
        if (melee) melee.EndAttack();
    }
}
