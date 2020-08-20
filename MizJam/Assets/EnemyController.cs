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

    

    public void Initialize(Action onDeath) {
        onDeath += moveableObject.LockHorizontalMovement;
        hittableObject.Initialize(onDeath);
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
