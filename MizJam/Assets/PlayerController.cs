using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider swordCollider;

    public float attackCooldown = 0.5f;
    private bool isAttacking = false;

    public float jumpSpeed = 7f;


    public PlayerMoveableObject moveableObject;


    public void Initialize() {
        this.moveableObject.Initialize(GameManager.Instance.gameController.mapScroller.mapMoveSpeed);
        this.moveableObject.UnlockHorizontalMovement();
    }

    public void LockHorizontalMovement() {
        this.moveableObject.LockHorizontalMovement();
        // TODO: Need to wait for camera
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && Input.GetKeyDown(KeyCode.Z)) {
            StartCoroutine(playAttackAnimation());
        }

        if (moveableObject.controller.isGrounded && Input.GetKeyDown(KeyCode.X)) {
            moveableObject.moveDirection.y = this.jumpSpeed;
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

    public void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger enter: " + other.tag);
        if (other.tag == "PlayerInteractable" || other.tag == "HittableObject") {
            PlayerInteractable interactable = other.GetComponent<PlayerInteractable>();
            interactable.Interact(this.moveableObject);
        }
    }

}
