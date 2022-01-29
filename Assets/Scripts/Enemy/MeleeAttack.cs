using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Player;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public bool attacking = false;
    [HideInInspector] public float knockback = 10f;
    [HideInInspector] public float damage = 10f;
    public float knockUp = 5f;
    public Transform myPos;

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player" && attacking) //player
        {
            attacking = false;

            var dir = myPos.forward;
            dir += new Vector3(0, knockUp, 0);
            //col.gameObject.GetComponentInParent<Rigidbody>().AddForce(dir*knockback);
            PlayerHealth.Current.Health -= damage;
        }
    }
}
