using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    double VELOCITY_LIMIT = 0.8;
    float ANGLE_LIMIT = 30.0f;
    double penartyTimes = 0.1;
    float penartyDist = 0.25f;
    double impact;

    public int Damage(Collider other, GameObject Player, EnemyConfig enemyConfig, GameObject Face) {

        impact = GetImpact(other);
        DebugWindow.instance.DFDebug(impact + "m/秒");
        //リミット以下の速度だとダメージは0
        if (impact < VELOCITY_LIMIT) {
            return 0;
        }
        //顔とのなす角がリミット以上だとダメージは0。
        //手の裏ではたくときのダメージが大きかったため
        /*
        float angle = GetAngleFromFaceToHand(Face, other);
        DebugWindow.instance.DFDebug("angle:" + angle);
        if ( angle > ANGLE_LIMIT) {
            return 0;
        }
        DebugWindow.instance.DFDebug("angle:" + angle);
        */
        int damage = GetDamage(
            impact,
            Player.GetComponent<PlayerConfig>().GetATK(),
            enemyConfig.GetDEF()
            );

        return damage;
    }

    public double GetImpact() {
        return impact;
    }

    //最後の吹っ飛ばし
    public void SetBlowOff(Collider other, float addForce) {
        List<Vector3> listPosition = other.gameObject.GetComponent<CalculationImpact>().GetListPosition();
        Vector3 direction = listPosition[0] - listPosition[1];
        direction.x *= addForce * (float)impact;
        direction.y *= addForce * (float)impact;
        direction.z *= addForce * (float)impact;
        GetComponent<Rigidbody>().AddForce(direction);
    }

    private float GetAngleFromFaceToHand(GameObject Face, Collider other) {
        //顔と手のなす角を求める
        return Mathf.Abs(Vector3.Angle(Face.transform.forward, other.gameObject.transform.forward) - 90.0f);
    }
    private double GetImpact(Collider other) {
        List<Vector3> listPosition = other.gameObject.GetComponent<CalculationImpact>().GetListPosition();
        int listCount = listPosition.Count;
        float dist = 0;
        for (int i = 0; i + 1 < listCount; i++) {
            dist += FQCommon.Common.GetDistance(listPosition[i], listPosition[i+1]);
        }
        //GetPunchType(listPosition[listCount - 1], listPosition[0]);
        //1秒間にどれだけの距離を進んだか返す。1フレーム当たり0.02秒
        double impact = dist * (1 / (listPosition.Count * 0.02));
        impact = CalcurationPenarty(listPosition, listCount, impact);
        return impact;
    }

    private int GetDamage(double impact, int atk, int def) {
        //impactを乗算する
        // atk = 攻撃力 * (1 + impactの10分の1)
        atk = (int)(Mathf.Ceil((float)(atk * (1 + impact * 0.1))));
        return FQCommon.Common.GetDamage(atk, def);
    }

    private double CalcurationPenarty(List<Vector3> listPosition, int listCount, double impact) {
        //ふり下ろし、ふり上げが一番強くなるのでペナルティをつける
        if (Mathf.Abs(listPosition[listCount - 1].y - listPosition[0].y) > penartyDist) {
            impact = impact * penartyTimes;
        }
        return impact;
    }

}
