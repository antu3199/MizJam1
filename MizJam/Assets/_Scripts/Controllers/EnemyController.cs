using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public HittableObject hittableObject;
    public EnemyMoveableObject moveableObject;
    public PlayerStats playerStats;
    public DealDamageInteractable damageInteractable;
    public SpriteRenderer enemySpriteRenderer;


    public double reference{get; set;}

    private CombatResObj combatResObjInstance;
    

    public void Initialize(Action onDeath, double reference, CombatResObj combatResObj) {
        this.combatResObjInstance = combatResObj;
        this.enemySpriteRenderer.sprite = combatResObj.gameSprite;
        this.reference = reference;
        onDeath += moveableObject.LockHorizontalMovement;
        onDeath += RemoveDamageHitbox;
        hittableObject.Initialize(onDeath, reference);
        this.playerStats.UpdateMaxHealth(reference);
        this.playerStats.SetValues(combatResObj.baseAttack, combatResObj.attackScaling, combatResObj.baseDefence, combatResObj.defenceScaling, combatResObj.baseMaxHealth, combatResObj.healthScaling);
        this.damageInteractable.Initialize(this);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerInteractable" || other.tag == "HittableObject") {
            PlayerInteractable interactable = other.GetComponent<PlayerInteractable>();
            if (interactable.affectsEnemy) {
                interactable.Interact(this.moveableObject);
            }
        }
    }

    public void SetAsBossEnemy() {
        PlayerStat attackStat = playerStats.GetRawStat(Stat.ATTACK);
        attackStat.value *= 2;

        PlayerStat healthStat = playerStats.GetRawStat(Stat.MAX_HEALTH);
        healthStat.value *= 100;

        this.playerStats.UpdateMaxHealth(this.reference);

        foreach (Reward reward in this.hittableObject.killRewards) {
            reward.value *= 100;
        }

        this.transform.localScale = new Vector3(2, 2, 1);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
    }

    private void RemoveDamageHitbox() {
        this.damageInteractable.canInteract = false;
    }
}
