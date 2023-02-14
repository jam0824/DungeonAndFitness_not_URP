using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownObject : MonoBehaviour
{
    public int hitCount = 5;
    public float FALL_POWER;
    public AudioClip fallDownSe;
    public AudioClip fallDownHitSe;
    public AudioSource audioSource;

    float colliderDeleteTime = 3.0f;
    bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �p���`�������̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "PlayerAttack") {
            ContactPoint contact = collision.contacts[0];
            MakeHitEffect(contact);
            makeHitSE("HitObjectSe");
            hitCount--;
            if ((hitCount <= 0)&&(!isFalling)) {
                isFalling = true;
                FallDown(contact);
            }
        }
    }

    /// <summary>
    /// �|�ꂽ���̏���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (isFalling) {
            if ((other.gameObject.tag == "Ground") || (other.gameObject.tag == "Wall")) {
                audioSource.PlayOneShot(fallDownHitSe);
                StartCoroutine(DeleteCollider(colliderDeleteTime));
            }
        }
    }

    /// <summary>
    /// �|��
    /// </summary>
    /// <param name="collision"></param>
    void FallDown(ContactPoint contact) {

        //Freeze����������
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;

        Vector3 direction = contact.normal;
        direction.x *= FALL_POWER;
        direction.z *= FALL_POWER;
        Vector3 pos = gameObject.transform.position;
        pos.y += 4.0f;
        //���̏�̕��ɗ͂������A���������ɓ|��
        GetComponent<Rigidbody>().AddForceAtPosition(direction, pos, ForceMode.Impulse);
        audioSource.PlayOneShot(fallDownSe);
    }

    void MakeHitEffect(ContactPoint contact) {
        Quaternion r = SingletonGeneral.instance.GetQuaternionFace();
        //�_���[�W�G�t�F�N�g
        GameObject hit = Instantiate(
            PlayerView.instance.config.PUNCH_HIT_PREFAB, 
            contact.point, 
            r);
    }

    void makeHitSE(string SeName) {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        //�_���[�W��
        SingletonGeneral.instance.PlayOneShot(audioSource, SeName);
    }

    IEnumerator DeleteCollider(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SingletonGeneral.instance.PlayOneShot(audioSource, "NormalEnemyDie");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
