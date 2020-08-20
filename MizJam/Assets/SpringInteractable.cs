using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringInteractable : PlayerInteractable
{

    public float bounceStrength = 2f;
    public override void Interact(PlayerController controller) {
        controller.moveDirection.x = bounceStrength;
    }
}
