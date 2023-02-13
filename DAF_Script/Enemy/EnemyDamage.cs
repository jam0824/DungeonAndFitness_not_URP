using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    float IMPACT_LIMIT = 800.0f;
    float MAX_DAMAGE_RATE = 5.0f;

    public int Damage(Collision collision, float impact, GameObject Player, EnemyConfig enemyConfig) {
        //リミット以下の速度だとダメージは0
        /*
        if (impact < IMPACT_LIMIT) {
            return 0;
        }
        */
        int damage = GetDamage(
            impact,
            Player.GetComponent<PlayerConfig>().GetATK(),
            enemyConfig.GetDEF()
            );

        return damage;
    }

    private int GetDamage(float impact, int atk, int def) {
        //impactを乗算する
        // atk = 攻撃力 * (1 + impactの1万分の1)
        float damageRate = (1 + impact * 0.0001f);
        //最大
        if (damageRate > MAX_DAMAGE_RATE) damageRate = MAX_DAMAGE_RATE;
        atk = (int)(Mathf.Ceil((float)(atk * damageRate)));
        return FQCommon.Common.GetDamage(atk, def);
    }

    


    public float GetImpact(Collision collision) {
        return collision.impulse.magnitude / Time.fixedDeltaTime;
    }

}
