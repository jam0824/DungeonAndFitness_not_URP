using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    float IMPACT_LIMIT = 1000.0f;

    public int Damage(Collision collision, float impact, GameObject Player, EnemyConfig enemyConfig) {
        //リミット以下の速度だとダメージは0
        if (impact < IMPACT_LIMIT) {
            return 0;
        }   
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
        atk = (int)(Mathf.Ceil((float)(atk * (1 + impact * 0.0001f))));
        return FQCommon.Common.GetDamage(atk, def);
    }

    


    public float GetImpact(Collision collision) {
        return collision.impulse.magnitude / Time.fixedDeltaTime;
    }

}
