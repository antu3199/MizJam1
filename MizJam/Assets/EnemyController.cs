using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public HittableObject hittableObject;
    public EnemyMoveableObject moveableObject;
    public PlayerStats playerStats;
    public PlayerInteractable interactable;

    private double reference;
    

    public void Initialize(Action onDeath, double reference) {
        this.reference = reference;
        onDeath += moveableObject.LockHorizontalMovement;
        hittableObject.Initialize(onDeath, reference);
        this.playerStats.UpdateMaxHealth(reference);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerInteractable" || other.tag == "HittableObject") {
            PlayerInteractable interactable = other.GetComponent<PlayerInteractable>();
            if (interactable.affectsEnemy) {
                interactable.Interact(this.moveableObject);
            }
        }
    }
}
