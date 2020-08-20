using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float knockback = 20f;
    private PlayerController controller;


    public void Initialize(PlayerController controller) {
        this.controller = controller;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "HittableObject") {
            HittableObject hittableObject = other.GetComponent<HittableObject>();

            controller.moveableObject.moveDirection.x = -knockback;
            hittableObject.GetHit(GameManager.Instance.gameState.playerStats.GetScaledStat(Stat.ATTACK, this.controller.GetReference()), this.knockback);
        }
    }
}
