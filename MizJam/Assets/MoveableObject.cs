using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveableObject : MonoBehaviour
{


    public float horizontalMoveSpeed = 1f;
    public float maxHorizontalMoveSpeed = 1f;

    protected bool horizontalMovement = false;


    [Header("Debug")]
    public Vector3 moveDirection;


    public void UnlockHorizontalMovement() {
        this.horizontalMovement = true;
    }

    public void LockHorizontalMovement() {
        this.horizontalMovement = false;
    }

}
