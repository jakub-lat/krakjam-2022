using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public bool attacking = false;
    public float knockback = 10f;

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player" && attacking) //player
        {
            attacking = false;
            col.gameObject.GetComponent<CharacterController>().Move((col.transform.position - transform.position).normalized * knockback);
        }
    }
}
