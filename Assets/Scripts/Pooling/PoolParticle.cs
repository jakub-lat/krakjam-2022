using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticle : MonoBehaviour
{
    public ParticleSystem ps;

    private void OnEnable()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Play();
        float d = Mathf.Max(ps.main.startLifetime.constantMax,ps.main.startLifetime.constant)+ps.main.duration;
        Invoke(nameof(Enqueue),d);
    }

    private void Enqueue()
    {
        GetComponent<PoolObj>().Enqueue();
    }
}
