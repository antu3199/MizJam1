using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HittableObject : MonoBehaviour
{
    public Animator animator;
    public double maxHealth = 1;
    public double health {get; set;}
    public PlayerStats playerStats;

    public List<Reward> killRewards;
    public Transform killRewardTransform;
    public PlayerInteractable interactable; // All hittable objects shoudl have a interactable script, even if it doesn't do anything.

    protected bool isDead = false;

    void Start() {
        this.Initialize();
    }

    public virtual void Initialize() {
        this.health = maxHealth;
        this.playerStats.Initialize();
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
        }
    }
}
