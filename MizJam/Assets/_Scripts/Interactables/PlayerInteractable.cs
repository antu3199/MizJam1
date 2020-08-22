using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public bool canInteract = true;
    public bool affectsEnemy = true;
    public virtual void Interact(MoveableObject controller) {
        // Override me!
    }
}
