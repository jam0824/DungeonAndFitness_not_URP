using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWalkAnim(bool isWalk) {
        anim.SetBool("Walk Forward", isWalk);
    }

    public bool GetBoolWalkAnim() {
        return anim.GetBool("Walk Forward");
    }

    public void setAttackAnim(string attackTrigerName) {
        anim.SetTrigger(attackTrigerName);
    }

    public void SetDamageAnim() {
        anim.SetTrigger("Take Damage");
    }

    public void setDieAnim() {
        anim.SetTrigger("Die");
    }

    public void DieDestroy() {
        Destroy(gameObject);
    }

    // Animation Event
    public void AlertObservers(string message) {

        if (message.Equals("AnimationDamageEnded")) {
            anim.ResetTrigger("Damage");
        }
    }
}
