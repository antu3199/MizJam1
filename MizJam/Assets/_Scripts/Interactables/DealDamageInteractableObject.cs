using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageInteractableObject : PlayerInteractable
{

    public double damageMultiplier = 10;

    public double reference;
    public bool disableAfterGettingHit = true;
    private Action onHit = null;

    public void Initialize(double damageReference, Action onHit) {
        this.reference = damageReference;
        this.onHit = onHit;
    }

    public override void Interact(MoveableObject controller) {
        if (!canInteract) {
            return;
        }

        // Instead of doing it the proper way, just use GetComponent to save time
        PlayerController playerController = controller.GetComponent<PlayerController>();
        if ( playerController != null) {
            playerController.DealDamageToMe(this.reference * damageMultiplier, 0);
            if (this.disableAfterGettingHit) {
                this.canInteract = false;
            }

            this.onHit();
        }
    }
}
