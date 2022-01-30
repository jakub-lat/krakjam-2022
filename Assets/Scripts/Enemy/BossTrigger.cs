using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public void TriggerBattle()
    {
        UI.ObjectivesUI.Current.SetObjective("MISSION: KILL THE BOSS", "SHOW YOUR TRUE RAGE");
        Boss.Current.StartBattle();
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
