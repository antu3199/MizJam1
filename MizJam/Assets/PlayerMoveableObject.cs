using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveableObject : MoveableObject
{

    [Header("PlayerMoveableObject")]
    public CharacterController controller;

    public float gravity = 9.8f;




    public void Initialize(float maxHorizontalMoveSpeed) {
        this.maxHorizontalMoveSpeed = maxHorizontalMoveSpeed;
    }


    // Update is called once per frame
    void Update()
    {

        if (!controller.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (this.horizontalMovement) {
            moveDirection.x += horizontalMoveSpeed;

            // Clamp speed to max
            moveDirection.x = Mathf.Min(this.maxHorizontalMoveSpeed, moveDirection.x);
        }

        controller.Move(moveDirection * Time.deltaTime);
    }
}
