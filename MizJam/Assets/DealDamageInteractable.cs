using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageInteractable : PlayerInteractable
{
    public double damage = 1;
    public bool pushBack = true;
    public float pushBackStrength = 20f;

    public void SetDamage(double damage) {
        this.damage = damage;
    }

    public override void Interact(MoveableObject controller) {
        // TODO: change params to fighting entity to implement this function
    }
}
