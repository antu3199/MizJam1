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

    public override void Interact(MoveableObject controller) {
        if (!hasTriggered) {


            if (this.triggerCallback != null) {
                this.triggerCallback();
            }

            this.hasTriggered = true;
        }

    }
}
