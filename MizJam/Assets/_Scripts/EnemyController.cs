﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public HittableObject hittableObject;
    public EnemyMoveableObject moveableObject;
    public PlayerStats playerStats;
    public DealDamageInteractable damageInteractable;


    public double reference{get; set;}
    

    public void Initialize(Action onDeath, double reference) {
        this.reference = reference;
        onDeath += moveableObject.LockHorizontalMovement;
        onDeath += RemoveDamageHitbox;
        hittableObject.Initialize(onDeath, reference);
        this.playerStats.UpdateMaxHealth(reference);
        this.damageInteractable.Initialize(this);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerInteractable" || other.tag == "HittableObject") {
            PlayerInteractable interactable = other.GetComponent<PlayerInteractable>();
            if (interactable.affectsEnemy) {
                interactable.Interact(this.moveableObject);
            }
        }
    }

    public void SetAsBossEnemy() {
        PlayerStat attackStat = playerStats.GetRawStat(Stat.ATTACK);
        attackStat.value *= 2;

        PlayerStat healthStat = playerStats.GetRawStat(Stat.MAX_HEALTH);
        healthStat.value *= 1000;
        foreach (Reward reward in this.hittableObject.killRewards) {
            reward.value *= 100;
        }
    }

    private void RemoveDamageHitbox() {
        this.damageInteractable.canInteract = false;
    }
}