using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public LayerMask destroyBullet;
    public string bulletholePoolingTag;

    public void OnTriggerEnter(Collider other)
    {
        if (destroyBullet == (destroyBullet | (1 << other.gameObject.layer)))
        {
            GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, transform.position, Quaternion.LookRotation(other.transform.position-transform.position));
            bulletHole.transform.parent = other.transform;
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
