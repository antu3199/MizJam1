using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMap : BasicMap
{
    public EnemyController enemy;
    public PlayerInteractable immovableWall;
    public CameraResetterInteractable cameraResetter;
    public bool mustKillEnemyToAdvance = true;
    public CombatRes combatRes;

    public Sprite bossEnemyMapSprite;
    protected LogController logController;

    private CombatResObj combatResInstance;

    public override void DoInitialize() {
        this.mapType = MapType.COMBAT;
        this.combatResInstance = this.combatRes.GetRandomResObject();
        this.levelIconSprite = this.combatResInstance.gameSprite;

        immovableWall.gameObject.SetActive(mustKillEnemyToAdvance);
        cameraResetter.gameObject.SetActive(!mustKillEnemyToAdvance);

        this.logController = GameManager.Instance.gameController.logController;
        this.enemy.Initialize(this.OnEnemyDeath, reference, this.combatResInstance);
        this.cameraResetter.Initialize(this.OnSkip);
    }

    public void BeginBattle() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
        StartCoroutine(this.beginTalkingCor());
    }

    public void SetAsBossMap() {
        this.mapType = MapType.COMBAT_BOSS;
        this.mustKillEnemyToAdvance = true;
        immovableWall.gameObject.SetActive(mustKillEnemyToAdvance);
        cameraResetter.gameObject.SetActive(!mustKillEnemyToAdvance);
        this.levelIconSprite = this.bossEnemyMapSprite;
        this.enemy.SetAsBossEnemy();
    }

    protected virtual IEnumerator beginTalkingCor() {
        yield return logController.TypeAnimation(new LogMessage(this.combatRes.GetRandomMessage(), this.combatResInstance.gameSprite));
 
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.gameController.playerController.UnlockHorizontalMovement();
        enemy.moveableObject.UnlockHorizontalMovement();
    }

    protected virtual void MoveMapAgain() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
        this.CallGoToNextMap();
    }

    protected virtual void OnEnemyDeath() {
        Debug.Log("On Enemy death");
        immovableWall.gameObject.SetActive(false);
        this.cameraResetter.gameObject.SetActive(true);

        
    }

    protected virtual void OnSkip() {
        enemy.moveableObject.LockHorizontalMovement();

        List<Transform> otherTransforms = new List<Transform>();
        otherTransforms.Add(enemy.transform);
        this.CallGoToNextMap();
        GameManager.Instance.gameController.mapScroller.ResetPlayerCamera(GameManager.Instance.gameController.playerController.moveableObject, otherTransforms);
    }
}
