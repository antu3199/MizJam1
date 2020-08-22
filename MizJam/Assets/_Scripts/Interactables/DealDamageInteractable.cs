using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageInteractable : PlayerInteractable
{
    public double damage = 1;
    public bool pushBack = true;

    private EnemyController enemyController;

    public void Initialize(EnemyController enemyController) {
        this.enemyController = enemyController;
    }

    public void SetDamage(double damage) {
        this.damage = damage;
    }

    public override void Interact(MoveableObject controller) {
        if (!canInteract) {
            return;
        }

        // Instead of doing it the proper way, just use GetComponent to save time
        PlayerController playerController = controller.GetComponent<PlayerController>();
        Debug.Log("Deal damage interactable");
        if ( playerController != null) {
            float knockback = 0;
            if (pushBack) {
                knockback = playerController.swordSwing.knockback;
            }

            Debug.Log("Deal damage interactable 2");
            double enemyToPlayerDamage = this.enemyController.playerStats.GetScaledStat(Stat.ATTACK, this.enemyController.reference);
            playerController.DealDamageToMe(enemyToPlayerDamage, knockback);


            double playerToEnemyDamage = playerController.swordSwing.GetSwordDamage()/5.0;
            this.enemyController.hittableObject.ManualGetHit(playerToEnemyDamage, knockback);
        }
    }
}
