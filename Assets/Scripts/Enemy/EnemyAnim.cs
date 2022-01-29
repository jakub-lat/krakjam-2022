using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Enemy e;

    private void Start()
    {
        e = GetComponentInParent<Enemy>();
    }
    public void Kaput()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void EndHit()
    {
        e.HitEnd();
    }
}
