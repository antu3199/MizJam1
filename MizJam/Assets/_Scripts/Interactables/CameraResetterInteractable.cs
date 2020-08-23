using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraResetterInteractable : PlayerInteractable
{

    private bool hasTriggered = false;
    private Action triggerCallback = null;

    public void Initialize(Action triggerCallback) {
        this.triggerCallback = triggerCallback;
    }

    void Update() {
        // Bandaid solution to fix some potential bugs
        if (!hasTriggered) {
            PlayerController player = GameManager.Instance.gameController.playerController;
            if (this.transform.position.x < player.transform.position.x || player.transform.position.y < 0) {
                this.triggerCallback();
                this.hasTriggered = true;
            }
        }
    }

    public override void Interact(MoveableObject controller) {
        if (!hasTriggered) {


            if (this.triggerCallback != null) {
                this.triggerCallback();
            }

            this.hasTriggered = true;
        }

    }
}
