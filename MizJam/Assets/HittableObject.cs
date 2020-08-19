using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    public Animator animator;
    public double maxHealth = 1;
    public double health {get; set;}
    public PlayerStats playerStats;

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
        }
    }
}
