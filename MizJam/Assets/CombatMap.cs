using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMap : BasicMap
{

    public List<LogMessage> messages;
    protected LogController logController;

    public override void DoInitialize() {
        this.logController = GameManager.Instance.gameController.logController;
    }

    public void BeginBattle() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
        StartCoroutine(this.beginTalkingCor());
    }

    protected IEnumerator beginTalkingCor() {

        foreach (LogMessage message in messages) {
            yield return logController.TypeAnimation(message);
        }

        // this.logController.ClearLogText();

        // this.MoveMapAgain();
    }

    protected virtual void MoveMapAgain() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
    }
}
