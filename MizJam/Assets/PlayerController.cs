using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider swordCollider;

    public float attackCooldown = 0.5f;
    private bool isAttacking = false;

    private int tmpTest = 0;

    public CharacterController controller;

    public float jumpSpeed = 1f;
    public float gravity = 9.8f;

    public float horizontalMoveSpeed = 1f;
    private bool horizontalMovement = false;
    private float maxHorizontalMoveSpeed = 1f;


    [Header("Debug")]
    public Vector3 moveDirection;


    public void Initialize() {
        this.maxHorizontalMoveSpeed = GameManager.Instance.gameController.mapScroller.mapMoveSpeed;

        this.UnlockHorizontalMovement(); // debug
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && Input.GetKeyDown(KeyCode.Z)) {
            StartCoroutine(playAttackAnimation());
        }

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.X)) {
            moveDirection.y = jumpSpeed;
        }

        if (!controller.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        moveDirection.x += horizontalMoveSpeed;

        // Clamp speed to max
        moveDirection.x = Mathf.Min(this.maxHorizontalMoveSpeed, moveDirection.x);

        controller.Move(moveDirection * Time.deltaTime);
    }

    public void UnlockHorizontalMovement() {
        this.horizontalMovement = true;
    }

    public void LockHorizontalMovement() {
        this.horizontalMovement = false;
        // TODO: Need to wait for camera
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
            interactable.Interact(this);
        }
    }

}
