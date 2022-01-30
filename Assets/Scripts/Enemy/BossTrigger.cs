using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossTrigger : MonoBehaviour
{
    public void TriggerBattle()
    {
        dir.Play();
        
    }

    private void Start()
    {
        dir.stopped += Director_stopped;
    }

    bool did = false;
    public PlayableDirector dir;
    private void Director_stopped(PlayableDirector dir) 
    {
        Boss.Current.StartBattle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!did && other.CompareTag("Player"))
        {
            did = true;
            TriggerBattle();
        }
    }
}
