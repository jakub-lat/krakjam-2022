using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticle : MonoBehaviour
{
    public ParticleSystem ps;
    public int particleIndex = 0;

    private void OnEnable()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        var main = ps.main;
        main.startColor = WorldChange.ChangeParticle.instance.curr[particleIndex];
        ps.Play();
        float d = Mathf.Max(ps.main.startLifetime.constantMax,ps.main.startLifetime.constant)+ps.main.duration;
        Invoke(nameof(Enqueue),d);
    }

    private void Enqueue()
    {
        GetComponent<PoolObj>().Enqueue();
    }
}
