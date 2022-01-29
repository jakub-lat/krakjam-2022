using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeData
{
    public Animator anim;
    public ParticleSystem dropParticles;
    public ParticleSystem brokenParticles;
}

public class BossPipe : MonoSingleton<BossPipe>
{
    public List<PipeData> pipes;
    private List<int> pipeHits;
    private List<bool> pipeToHit;

    public int emissionStart = 10;
    public int emissionStep = 10;
    public int hitsToDestroy = 4;

    private void Start()
    {
        pLeft = 0;
        pipeHits = new List<int>();
        pipeToHit = new List<bool>();
        foreach(PipeData p in pipes)
        {
            p.dropParticles.Stop();
            pipeHits.Add(hitsToDestroy);
            pipeToHit.Add(false);
            pLeft++;
        }
    }

    int pLeft = 0;
    public void NextPipe()
    {
        if (pLeft <= 0) return;

        pLeft--;
        int i = Random.Range(0, pipeToHit.Count);
        while(pipeToHit[i]) { i = Random.Range(0, pipeToHit.Count); }

        pipeToHit[i] = true;
        PipeData p = pipes[i];
        p.dropParticles.Play();
        var main = p.dropParticles.emission;
        main.rateOverTime = emissionStart;
    }

    public void PipeHit(int i)
    {
        if (pipeHits[i] <= 0) return;

        pipeHits[i]--;
        if (pipeHits[i] <= 0)
        {
            pipes[i].brokenParticles.Play();
            var emmision = pipes[i].dropParticles.emission;
            emmision.rateOverTime = 0;

            //damage boss
            Boss.Current.PipeHit();

            return;
        }

        var main = pipes[i].dropParticles.emission;
        main.rateOverTime = emissionStart+(hitsToDestroy- pipeHits[i])*emissionStep;
    }
}
