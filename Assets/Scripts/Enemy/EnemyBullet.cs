using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public LayerMask destroyBullet;
    public string bulletholePoolingTag;

    public void OnCollisionEnter(Collision col)
    {
        if (destroyBullet == (destroyBullet | (1 << col.gameObject.layer)))
        {
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (col.transform.tag != "Player") //if not a player spawn a bullethole
            {
                GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, pos, rot);
                bulletHole.transform.parent = col.transform;
            }
            Enqueue();
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(Enqueue),20f);
    }

    private void Enqueue()
    {
        if (GetComponent<PoolObj>()) GetComponent<PoolObj>().Enqueue();
        else Destroy(gameObject);
    }
}
