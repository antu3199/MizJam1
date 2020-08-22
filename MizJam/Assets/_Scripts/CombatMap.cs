using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMap : BasicMap
{

    public List<LogMessage> messages;
    public EnemyController enemy;
    public PlayerInteractable immovableWall;
    public CameraResetterInteractable cameraResetter;
    public bool mustKillEnemyToAdvance = true;
    public CombatMapRes combatMapRes;
    protected LogController logController;

    public override void DoInitialize() {
        this.mapType = MapType.COMBAT;
        immovableWall.gameObject.SetActive(mustKillEnemyToAdvance);
        cameraResetter.gameObject.SetActive(!mustKillEnemyToAdvance);

        this.logController = GameManager.Instance.gameController.logController;
        this.enemy.Initialize(this.OnEnemyDeath, reference);
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
        this.levelIconSprite = this.combatMapRes.bossMap;
        this.enemy.SetAsBossEnemy();
    }

    protected virtual IEnumerator beginTalkingCor() {
        foreach (LogMessage message in messages) {
            yield return logController.TypeAnimation(message);
        }
 
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.gameController.playerController.UnlockHorizontalMovement();
        enemy.moveableObject.UnlockHorizontalMovement();


        // this.logController.ClearLogText();

        // this.MoveMapAgain();
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
