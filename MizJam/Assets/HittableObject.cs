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

    public double numTimesHit{get; set;}

    public PlayerCanvas enemyCanvas;

    public Transform damageTextTransform;


    protected bool isDead = false;

    protected Action onDeath = null;

    protected bool initialized = false;
    protected double reference = 0;



    void Start() {
        this.Initialize(null);
    }

    public virtual void Initialize(Action onDeath, double reference = 0) {
        if (initialized) {
            if (this.onDeath == null && onDeath != null) {
                this.onDeath = onDeath;
                this.reference = reference;
            }
            return;
        }

        this.onDeath = onDeath;
        this.reference = reference;
        this.playerStats.Initialize();
        this.numTimesHit = 1;

        this.initialized = true;
    }

    public virtual void GetHit(double damage, float knockback, bool increaseRewardForHit = true) {
        if (isDead == true) {
            return;
        }

        double realDamage = this.playerStats.DealDamageToMe(damage, reference);

        GameManager.Instance.gameController.InstantiateDamageText(realDamage, false, this.damageTextTransform.position, this.damageTextTransform);

        if (this.enemyCanvas != null) {
            double t = this.playerStats.hp / this.playerStats.GetScaledStat(Stat.MAX_HEALTH, reference);
            this.enemyCanvas.SetHPProgress((float)t);
        }


        if (increaseRewardForHit) {
            this.numTimesHit++;
        }

        if (this.playerStats.hp <= 0) {
            isDead = true;
            animator.Play("Death");

            foreach (Reward killReward in killRewards) {
                killReward.value *= this.numTimesHit;
            }

            StartCoroutine(GameManager.Instance.gameController.InstantiateRewardCor(killRewards, killRewardTransform.position, killRewardTransform));
            if (this.onDeath != null) {
                this.onDeath();
            }
        } else {
            MoveableObject moveableObject = GetComponent<MoveableObject>();
            if (moveableObject != null) {
                moveableObject.moveDirection.x = knockback;
            }
        }
    }

    public void ManualGetHit(double damage, float knockback) {
        this.GetHit(damage, knockback, false);
    } 
}
