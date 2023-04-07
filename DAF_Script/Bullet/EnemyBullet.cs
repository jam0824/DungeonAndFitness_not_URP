using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public GameObject HitPrefab;
    public GameObject player { set; get; }
    GameObject Enemy;
    int ATK = 0;
    float speed = 1.0f;
    float bulletWaitTime = 2.0f;
    float bulletSize = 1.0f;
    const float DESTROY_COUNT = 20.0f;
    Vector3 addScale;
    public EnemyConfig enemyConfig { set; get; }
    public GameObject firePosition { set; get; }
    bool isBecomeShotBig = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
        float add = bulletSize / (bulletWaitTime * 72f);
        addScale = new Vector3(add, add, add);
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        StartCoroutine(IEShot());
        StartCoroutine(IEDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        //ìGÇ™éÄÇÒÇæÇÁíeÇè¡Ç∑
        if (enemyConfig.GetHP() <= 0) HitBullet();
        //flagÇ™óßÇ¬Ç‹Ç≈ÇÕíeÇëÂÇ´Ç≠Ç∑ÇÈ
        if (!isBecomeShotBig) {
            BecomeShotBig(addScale);
            gameObject.transform.position = firePosition.transform.position;
            transform.LookAt(player.transform);
        }
    }

    void BecomeShotBig(Vector3 addScale) {
        Vector3 scale = transform.localScale;
        scale += addScale;
        transform.localScale = scale;
    }

    IEnumerator IEShot() {
        yield return new WaitForSeconds(bulletWaitTime);

        isBecomeShotBig = true;
        Rigidbody rigidBullet = GetComponent<Rigidbody>();
        AudioSource audioSource = GetComponent<AudioSource>();
        rigidBullet.velocity = transform.forward.normalized * speed;
        SingletonGeneral.instance.PlayOneShot(audioSource, "NormalShot");
        Enemy.GetComponent<EnemyAnimation>().setAttackAnim("Attack 01");
    }
    IEnumerator IEDestroy() {
        yield return new WaitForSeconds(DESTROY_COUNT);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        //ï«Ç‚è∞Ç≈è¡Ç∑
        if ((other.gameObject.tag == "Wall") || 
            (other.gameObject.tag == "Ground")) {
            HitBullet();
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

    public void SetWaitTime(float second) {
        bulletWaitTime = second;
    }

    public void SetEnemyGameObject(GameObject obj) {
        Enemy = obj;
    }

    public void SetLocalSize(float size) {
        bulletSize = size;
    }
}
