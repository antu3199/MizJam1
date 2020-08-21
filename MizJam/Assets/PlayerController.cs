﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider swordCollider;

    public float attackCooldown = 0.5f;
    public bool canMove = true;

    private bool isAttacking = false;

    public float jumpSpeed = 7f;


    public SwordSwing swordSwing;
    public PlayerMoveableObject moveableObject;
    public double GPSScaler = 300;

    public PlayerCanvas playerCanvas;

    public Transform damageTextTransform;

    public void Initialize() {
        this.swordSwing.Initialize(this);
        this.moveableObject.Initialize(GameManager.Instance.gameController.mapScroller.mapMoveSpeed);
        //this.moveableObject.UnlockHorizontalMovement();
    }

    public void UnlockHorizontalMovement() {
        this.moveableObject.UnlockHorizontalMovement();
    }

    public void LockHorizontalMovement() {
        this.moveableObject.LockHorizontalMovement();
        // TODO: Need to wait for camera
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) {
            return;
        }

        if (isAttacking == false && Input.GetKeyDown(KeyCode.Z)) {
            StartCoroutine(playAttackAnimation());
        }

        if (moveableObject.controller.isGrounded && Input.GetKeyDown(KeyCode.X)) {
            moveableObject.moveDirection.y = this.jumpSpeed;
        }

    }

    public double GetReference() {
        return GameManager.Instance.gameState.GPS * this.GPSScaler;
    }

    public void DealDamageToMe(double damage, float knockback) {
        double realDamage = GameManager.Instance.gameState.playerStats.DealDamageToMe(damage, this.GetReference());
        this.moveableObject.moveDirection.x = -knockback;
        if (GameManager.Instance.gameState.playerStats.hp <= 0) {
            this.canMove = false;
            this.moveableObject.LockHorizontalMovement();
            this.animator.StopPlayback();
            this.animator.Play("Death");
            GameManager.Instance.gameController.OnPlayerDeath();
        }

        Debug.Log("Player remaining Hp: " + GameManager.Instance.gameState.playerStats.hp + "/" + GameManager.Instance.gameState.playerStats.GetScaledStat(Stat.MAX_HEALTH, GameManager.Instance.gameState.GPS));

        double t = GameManager.Instance.gameState.playerStats.hp / GameManager.Instance.gameState.playerStats.GetScaledStat(Stat.MAX_HEALTH, GameManager.Instance.gameState.GPS);
        this.playerCanvas.SetHPProgress((float)t);
        GameManager.Instance.gameController.InstantiateDamageText(realDamage, true, this.damageTextTransform.position, this.damageTextTransform);
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
        if (!canMove) {
            return;
        }

        if (other.tag == "PlayerInteractable" || other.tag == "HittableObject") {
            Debug.Log("Trigger enter: " + other.tag);
            PlayerInteractable interactable = other.GetComponent<PlayerInteractable>();
            interactable.Interact(this.moveableObject);
        }
    }

}
