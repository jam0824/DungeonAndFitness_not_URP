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
        DebugWindow.instance.DFDebug(impact + "m/�b");
        //���~�b�g�ȉ��̑��x���ƃ_���[�W��0
        if (impact < VELOCITY_LIMIT) {
            return 0;
        }
        //��Ƃ̂Ȃ��p�����~�b�g�ȏゾ�ƃ_���[�W��0�B
        //��̗��ł͂����Ƃ��̃_���[�W���傫����������
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

    //�Ō�̐�����΂�
    public void SetBlowOff(Collider other, float addForce) {
        List<Vector3> listPosition = other.gameObject.GetComponent<CalculationImpact>().GetListPosition();
        Vector3 direction = listPosition[0] - listPosition[1];
        direction.x *= addForce * (float)impact;
        direction.y *= addForce * (float)impact;
        direction.z *= addForce * (float)impact;
        GetComponent<Rigidbody>().AddForce(direction);
    }

    private float GetAngleFromFaceToHand(GameObject Face, Collider other) {
        //��Ǝ�̂Ȃ��p�����߂�
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
        //1�b�Ԃɂǂꂾ���̋�����i�񂾂��Ԃ��B1�t���[��������0.02�b
        double impact = dist * (1 / (listPosition.Count * 0.02));
        impact = CalcurationPenarty(listPosition, listCount, impact);
        return impact;
    }

    private int GetDamage(double impact, int atk, int def) {
        //impact����Z����
        // atk = �U���� * (1 + impact��10����1)
        atk = (int)(Mathf.Ceil((float)(atk * (1 + impact * 0.1))));
        return FQCommon.Common.GetDamage(atk, def);
    }

    private double CalcurationPenarty(List<Vector3> listPosition, int listCount, double impact) {
        //�ӂ艺�낵�A�ӂ�グ����ԋ����Ȃ�̂Ńy�i���e�B������
        if (Mathf.Abs(listPosition[listCount - 1].y - listPosition[0].y) > penartyDist) {
            impact = impact * penartyTimes;
        }
        return impact;
    }

}
