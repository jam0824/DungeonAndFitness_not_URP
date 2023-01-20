using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    int NORMAL_SHOT = 0;
    int NORMAL_HIT = 1;

    public GameObject HitPrefab;
    public GameObject player { set; get; }
    public GeneralSystem generalSystem { set; get; }
    GameObject Enemy;
    Rigidbody rigidBullet;
    int ATK = 0;
    float speed = 1.0f;
    int count = 0;
    float waitCount = 120.0f;
    float destroyCount = 1400.0f;
    Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 addScale;
    AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidBullet = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        float add = 1.0f / waitCount;
        addScale = new Vector3(add, add, add);

    }

    // Update is called once per frame
    void Update()
    {
        if (count == waitCount) {
            rigidBullet.velocity = transform.forward.normalized * speed;
            generalSystem.PlayOneShot(audioSource, "NormalShot");
            Enemy.GetComponent<EnemyAnimation>().setAttackAnim("Attack 01");
        }
        else if (count < waitCount) {
            scale += addScale;
            transform.localScale = scale;
            transform.LookAt(player.transform);
        }
        count++;
        if(count >= destroyCount) {
            Destroy(gameObject);
        }

    }

    public void HitBullet() {
        Vector3 pos = transform.position;
        Vector3 addPos = -transform.forward;
        addPos.y = 0;
        pos += addPos;
        GameObject shot = Instantiate(HitPrefab, pos, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetATK(int atk) {
        ATK = atk;
    }

    public int GetATK() {
        return ATK;
    }

    public void SetSPD(float spd) {
        speed = spd;
    }

    public void SetWaitCount(float count) {
        waitCount = count;
    }

    public void SetEnemyGameObject(GameObject obj) {
        Enemy = obj;
    }
}
