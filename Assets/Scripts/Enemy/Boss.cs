using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoSingleton<Boss>
{
    private bool battle = false;
    private bool dead = false;
    private float health;

    public float startingHealth = 600;
    public Image healthBar;
    public Canvas BossUI;

    public void StartBattle()
    {
        health = startingHealth;
        dead = false;
        BossUI.enabled = true;
        healthBar.fillAmount = startingHealth / health;
        EnemySpawner.Current.KillAll();

        battle = true;
    }

    private void Start()
    {
        BossUI.enabled = false;
    }

    private void Update()
    {
        if (dead || !battle) return;
    }

    public void GotHit(float amount)
    {
        if (dead || !battle) return;

        health -= amount;
        if (health <= 0)
        {
            dead = true;
            health = 0;
        }

        healthBar.fillAmount = health/ startingHealth;
    }
}
