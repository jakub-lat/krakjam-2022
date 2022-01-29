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

        Debug.Log("nn");

        pLeft--;
        int i = Random.Range(0, pipeToHit.Count);
        while(pipeToHit[i]) { i = Random.Range(0, pipeToHit.Count); }

        pipeToHit[i] = true;
        Debug.Log(i);

        pipes[i].anim.Play("Pumping");
        pipes[i].dropParticles.Play();
        var main = pipes[i].dropParticles.emission;
        main.rateOverTime = emissionStart;
    }

    public bool PipeHit(int i)
    {
        if (pipeHits[i] <= 0 || !pipeToHit[i]) return false;

        pipeHits[i]--;
        if (pipeHits[i] <= 0)
        {
            pipes[i].brokenParticles.Play();
            var emmision = pipes[i].dropParticles.emission;
            emmision.rateOverTime = 0;

            pipes[i].anim.Play("Broken");

            //damage boss
            Boss.Current.PipeHit();

            return true;
        }

        var main = pipes[i].dropParticles.emission;
        main.rateOverTime = emissionStart+(hitsToDestroy- pipeHits[i])*emissionStep;
        return true;
    }
}
