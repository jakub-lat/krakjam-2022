using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossTrigger : MonoBehaviour
{
    public void TriggerBattle()
    {
        Boss.Current.StartBattle();
        UI.ObjectivesUI.Current.SetObjective("MISSION: KILL THE BOSS", "SHOW YOUR TRUE RAGE");
    }

    bool did = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!did && other.CompareTag("Player"))
        {
            did = true;
            TriggerBattle();
        }
    }
}
