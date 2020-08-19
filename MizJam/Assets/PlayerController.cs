using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider2D swordCollider;

    public float attackCooldown = 0.5f;
    private bool isAttacking = false;

    private int tmpTest = 0;

    void Start() {
    }
    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && Input.GetKeyDown(KeyCode.Z)) {

            StartCoroutine(playAttackAnimation());
            //animator.Play("Attack");
        }

     

    }

    private IEnumerator playAttackAnimation() {
            this.isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
            yield return null;
            int layer = animator.GetLayerIndex("AttackLayer");
            while(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1.0f) {
                yield return null;
            }

            this.isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
    }

    public void AttackHitboxActivate() {
        this.swordCollider.enabled = true;
    }

    public void AttackHitboxDeactivate() {
       // this.swordCollider.enabled = false;
    }

}
