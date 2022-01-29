using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public LayerMask destroyBullet;
    public string bulletholePoolingTag;
    public string hitParticlePoolingTag;
    public float damage;

    public void OnCollisionEnter(Collision col)
    {
        if (destroyBullet == (destroyBullet | (1 << col.gameObject.layer)))
        {
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (col.transform.tag == "Player")
            {
                GameObject particle = ObjectPooler.Current.SpawnPool(hitParticlePoolingTag, pos,
                    Quaternion.Euler(col.transform.position - transform.position));
                PlayerHealth.Current.Health -= damage;
            }
            else
            {
                GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, pos, rot);
                bulletHole.transform.parent = col.transform;
            }

            Enqueue();
            stilEnqueue = false;
        }
    }

    bool stilEnqueue = false;
    private void OnEnable()
    {
        stilEnqueue = true;
        Invoke(nameof(Enqueue), 20f);
    }

    private void Enqueue()
    {
        if (!stilEnqueue) return;
        if (GetComponent<PoolObj>()) GetComponent<PoolObj>().Enqueue();
        else Destroy(gameObject);
    }
}
