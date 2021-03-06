﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveableObject : MoveableObject
{
    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if (this.horizontalMovement) {
            moveDirection.x += horizontalMoveSpeed * Time.deltaTime;

            // Clamp speed to max
            moveDirection.x = Mathf.Max(-this.maxHorizontalMoveSpeed, moveDirection.x);
        } else {
            this.moveDirection = Vector3.zero;
        }


        this.transform.position += moveDirection * Time.deltaTime;
    }
}
