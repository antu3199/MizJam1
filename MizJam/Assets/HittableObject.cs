using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    public Animator animator;
    public double maxHealth = 1;
    public double health {get; set;}

    protected bool isDead = false;

    void Start() {
        this.Initialize();
    }

    public virtual void Initialize() {
        this.health = maxHealth;
    }


    public virtual void GetHit(double damage) {
        if (isDead == true) {
            return;
        }

        health -= damage;
        if (health <= 0) {
            isDead = true;
            animator.Play("Death");
        }
    }

}
