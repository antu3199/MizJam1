using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other) {
        if (other.tag == "HittableObject") {
            HittableObject hittableObject = other.GetComponent<HittableObject>();

            hittableObject.GetHit(1);
        }
    }
}
