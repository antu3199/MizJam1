using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class HittableObject : MonoBehaviour
{
    public Animator animator;
    public PlayerStats playerStats;

    public List<Reward> killRewards;
    public Transform killRewardTransform;
    public PlayerInteractable interactable; // All hittable objects shoudl have a interactable script, even if it doesn't do anything.

    protected bool isDead = false;

    protected Action onDeath = null;

    protected bool initialized = false;

    void Start() {
        this.Initialize(null);
    }

    public virtual void Initialize(Action onDeath) {
        if (initialized) {
            if (this.onDeath == null && onDeath != null) {
                this.onDeath = onDeath;
            }
            return;
        }

        this.onDeath = onDeath;
        this.playerStats.Initialize();

        this.initialized = true;
    }

    public virtual void GetHit(double damage) {
        if (isDead == true) {
            return;
        }

        this.playerStats.DealDamageToMe(damage);

        if (this.playerStats.hp <= 0) {
            isDead = true;
            animator.Play("Death");
            StartCoroutine(GameManager.Instance.gameController.InstantiateRewardCor(killRewards, killRewardTransform.position, killRewardTransform));
            if (this.onDeath != null) {
                this.onDeath();
            }
        }
    }
}
