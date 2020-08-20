using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public virtual void Interact(MoveableObject controller) {
        // Override me!
    }
}
