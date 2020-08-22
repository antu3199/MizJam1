using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringInteractable : PlayerInteractable
{

    public float bounceStrength = 2f;
    public override void Interact(MoveableObject controller) {
        if (Mathf.Sign(controller.horizontalMoveSpeed) == Mathf.Sign(bounceStrength)) {
            controller.moveDirection.x = 1 * Mathf.Sign(controller.horizontalMoveSpeed);
        } else {
            controller.moveDirection.x = bounceStrength;
        }
    }

    
}
