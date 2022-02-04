using System;
using System.Linq;
using DefaultNamespace.Enemy;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDamageUtils
{
    public static void EnemyDamage(GameObject go, Vector3 hitPoint, Vector3 hitNormal, float normalDamage, float headshotDamage, float damageRandomness, Func<bool, AudioClip> soundGetter = null)
    {
        if (normalDamage == 0) return;

        var isNormalHit = go.CompareTag("Enemy");
        var isHeadshotHit = go.CompareTag("EnemyHead");
        var isHit = isNormalHit || isHeadshotHit;

        if (!isHit) return;

        var enemy = go.GetComponentInParent<IEnemy>();
        if (enemy == null) return;
        
        var damageRandom = Random.Range(-damageRandomness, damageRandomness);
        var damage = (int)(isHeadshotHit ? headshotDamage : normalDamage);
        damage += (int)damageRandom;
        
        enemy.GotHit(damage);
        if(soundGetter != null) enemy.PlaySound(soundGetter(isHeadshotHit));

        ObjectPooler.Current.SpawnPool("HitParticle", hitPoint,
            Quaternion.LookRotation(hitNormal));

        if(isHeadshotHit) {
            HitmarkManager.Current.GetHeadshotHit();
            PopupManager.Current.SpawnHeadshotDamage(go.transform, damage);
            Scoreboard.GameScoreboard.Current.levelData.headshots++;
            LevelManager.Current.Score += LevelManager.Current.headshotScore;
        }
        else
        {
            HitmarkManager.Current.GetNormalHit();
            PopupManager.Current.SpawnStandardDamage(go.transform, damage);
        }
    }

    public static void EnemyDamage(RaycastHit hit, float normalDamage, float headshotDamage, float damageRandomness,
        Func<bool, AudioClip> soundGetter = null)
    {
        EnemyDamage(hit.collider.gameObject, hit.point, hit.normal, normalDamage, headshotDamage, damageRandomness, soundGetter);
    }
    
    public static void EnemyDamage(Collision col, float normalDamage, float headshotDamage, float damageRandomness,
        Func<bool, AudioClip> soundGetter = null)
    {
        var contact = col.contacts.FirstOrDefault();
        EnemyDamage(col.gameObject, contact.point, contact.normal, normalDamage, headshotDamage, damageRandomness, soundGetter);
    }
}
