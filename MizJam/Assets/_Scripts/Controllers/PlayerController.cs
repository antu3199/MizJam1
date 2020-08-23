using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider swordCollider;

    public float attackCooldown = 0.5f;
    public bool canMove = true;

    private bool isAttacking = false;

    public float jumpSpeed = 7f;


    public SwordSwing swordSwing;
    public PlayerMoveableObject moveableObject;
    public double GPSScaler = 300;

    public PlayerCanvas playerCanvas;

    public Transform damageTextTransform;

 
    public GameObject exclaimationPoint;

    public AudioClip swordSwingSoundEffect;
    public AudioClip notificationSoundEffect;



    private Action jumpOverride = null;
    private Action cancelOverride = null;

    public float autoTimerTime = 5f;
    private float autoTimerCounter;
    
    private string jumpOverrideString;
    private string cancelOverrideString;

    private const string JUMP_LABEL = "X:JUMP";

    private List<ActionItemUI> actionItems;


    public List<Sprite> armorSprites;
    public SpriteRenderer armor;

    public void Initialize() {
        this.actionItems = GameManager.Instance.gameController.logController.actionItems;

        this.swordSwing.Initialize(this);
        this.moveableObject.Initialize(GameManager.Instance.gameController.mapScroller.mapMoveSpeed);

        this.actionItems[1].SetLabel(JUMP_LABEL);
        this.actionItems[2].gameObject.SetActive(false);

        this.SetArmor(null);
        Messenger.AddListener<ItemUpdate>(Messages.OnItemBuy, this.SetArmor);

        //this.moveableObject.UnlockHorizontalMovement();
    }

    private void SetArmor(ItemUpdate update) {
        // For simplicity just recalculate
        int maxOwned = 0;
        int totalItemsInShop = GameManager.Instance.gameData.marketItems.Count;
        int increment = totalItemsInShop/4;

        for (int i = 0; i < totalItemsInShop; i++) {
            if (GameManager.Instance.gameState.items[i].owned >= 1) {
                maxOwned = i;
            }
        }

        Sprite spriteToUse = this.armorSprites[0];
        if (maxOwned >= 3 * increment) {
            spriteToUse = this.armorSprites[3];
        } else if (maxOwned >= 2 * increment) {
            spriteToUse = this.armorSprites[2];
        } else if (maxOwned >= 1 * increment) {
            spriteToUse = this.armorSprites[1];
        } else {
            spriteToUse = this.armorSprites[0];
        }

        this.armor.sprite = spriteToUse;
    }


    void OnDestroy() {

        Messenger.RemoveListener<ItemUpdate>(Messages.OnItemBuy, this.SetArmor);

        if (this.jumpOverride != null || this.cancelOverride != null) {
            this.ResetOverrides();
        }
    }

    public void UnlockHorizontalMovement() {
        this.moveableObject.UnlockHorizontalMovement();
    }

    public void LockHorizontalMovement() {
        this.moveableObject.LockHorizontalMovement();
        // TODO: Need to wait for camera
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) {
            return;
        }

        /*
        if (this.transform.position.x < -20f) {
            GameManager.Instance.gameController.mapScroller.ResetPlayerCamera(this.moveableObject, new List<Transform>(), true);
            return;
        }
        */


        if (isAttacking == false && Input.GetKeyDown(KeyCode.Z)) {
            GameManager.Instance.audio.PlayOneShot(this.swordSwingSoundEffect);
            StartCoroutine(playAttackAnimation());
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            this.DoX();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            this.DoC();
        }

        // For simplicity, auto timer is hardcoded to C.
        if (this.cancelOverride != null) {
            this.autoTimerCounter += Time.deltaTime;
            float t = this.autoTimerCounter / this.autoTimerTime;
            this.actionItems[2].SetProgressBarValue(t);
            if (this.autoTimerCounter >= this.autoTimerTime) {
                this.autoTimerCounter = 0;
                this.DoC();
                this.ResetOverrides();
            }
        }


    }

    private void DoX() {
        if (this.jumpOverride == null && moveableObject.controller.isGrounded ) {
            moveableObject.moveDirection.y = this.jumpSpeed;
        } else if (this.jumpOverride != null) {
            this.jumpOverride();
        }
    }

    private void DoC() {
        if (this.cancelOverride != null) {
            this.cancelOverride();
        }
    }

    public double GetReference() {
        return GameManager.Instance.gameState.GPS * this.GPSScaler;
    }

    public void DealDamageToMe(double damage, float knockback) {
        double realDamage = GameManager.Instance.gameState.playerStats.DealDamageToMe(damage, this.GetReference());
        if (knockback != 0) {
            this.moveableObject.moveDirection.x = -knockback;
        }

        if (GameManager.Instance.gameState.playerStats.hp <= 0) {
            this.canMove = false;
            this.moveableObject.LockHorizontalMovement();
            this.animator.StopPlayback();
            this.animator.Play("Death");
            GameManager.Instance.gameController.OnPlayerDeath();
        }

        Debug.Log("Player remaining Hp: " + GameManager.Instance.gameState.playerStats.hp + "/" + GameManager.Instance.gameState.playerStats.GetScaledStat(Stat.MAX_HEALTH, GameManager.Instance.gameState.GPS));

        double t = GameManager.Instance.gameState.playerStats.hp / GameManager.Instance.gameState.playerStats.GetScaledStat(Stat.MAX_HEALTH, GameManager.Instance.gameState.GPS);
        this.playerCanvas.SetHPProgress((float)t);
        GameManager.Instance.gameController.InstantiateDamageText(realDamage, true, this.damageTextTransform.position, this.damageTextTransform);
    }

    private IEnumerator playAttackAnimation() {
            this.isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
            yield return null;
            int layer = animator.GetLayerIndex("AttackLayer");
            while(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1.0f) {
                yield return null;
            }

            this.isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
    }

    public void AttackHitboxActivate() {
        this.swordCollider.enabled = true;
    }

    public void AttackHitboxDeactivate() {
       // this.swordCollider.enabled = false;
    }

    public void OnTriggerEnter(Collider other) {
        if (!canMove) {
            return;
        }

        if (other.tag == "PlayerInteractable" || other.tag == "HittableObject") {
            PlayerInteractable interactable = other.GetComponent<PlayerInteractable>();
            interactable.Interact(this.moveableObject);
        }
    }

    public void OverrideJumpButton(Action jumpOverride, string label, bool defaultChoice = false) {
        this.jumpOverride = jumpOverride;
        this.actionItems[1].gameObject.SetActive(true);
        this.actionItems[1].SetProgressBarVisible(defaultChoice);
        this.actionItems[1].SetLabel("X:" + label);
        this.exclaimationPoint.gameObject.SetActive(true);

        GameManager.Instance.audio.PlayOneShot(this.notificationSoundEffect);

        if (defaultChoice) {
            this.autoTimerCounter = 0;
        }
    }

    public void OverrideCancelButton(Action cancelOverride, string label, bool defaultChoice = false) {
        this.cancelOverride = cancelOverride;
        this.actionItems[2].gameObject.SetActive(true);
        this.actionItems[2].SetProgressBarVisible(defaultChoice);
        this.actionItems[2].SetLabel("C:" + label);
        this.exclaimationPoint.gameObject.SetActive(true);

        if (defaultChoice) {
            this.autoTimerCounter = 0;
        }
    }

    public void ResetOverrides() {
        this.jumpOverride = null;
        this.cancelOverride = null;
        this.actionItems[1].SetProgressBarVisible(false);
        this.actionItems[1].SetLabel(JUMP_LABEL);
        this.actionItems[2].gameObject.SetActive(false);
        this.exclaimationPoint.gameObject.SetActive(false);

    }

}
