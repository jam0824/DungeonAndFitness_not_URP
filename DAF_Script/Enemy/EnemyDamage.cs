using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    float IMPACT_LIMIT = 800.0f;
    float MAX_DAMAGE_RATE = 5.0f;

    public int Damage(Collision collision, float impact, GameObject Player, EnemyConfig enemyConfig) {
        //���~�b�g�ȉ��̑��x���ƃ_���[�W��0
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
        //impact����Z����
        // atk = �U���� * (1 + impact��1������1)
        float damageRate = (1 + impact * 0.0001f);
        //�ő�
        if (damageRate > MAX_DAMAGE_RATE) damageRate = MAX_DAMAGE_RATE;
        atk = (int)(Mathf.Ceil((float)(atk * damageRate)));
        return FQCommon.Common.GetDamage(atk, def);
    }

    


    public float GetImpact(Collision collision) {
        return collision.impulse.magnitude / Time.fixedDeltaTime;
    }

}
