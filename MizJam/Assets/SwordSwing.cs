using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "HittableObject") {
            HittableObject hittableObject = other.GetComponent<HittableObject>();

            hittableObject.GetHit(1);
        }
    }
}
