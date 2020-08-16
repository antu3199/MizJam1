using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingMap : BasicMap
{
    // Start is called before the first frame update

    public List<string> messages;
    protected LogController logController;

    public override void DoInitialize() {
        this.logController = GameManager.Instance.gameController.logController;
    }

    public void BeginTalking() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
        StartCoroutine(this.beginTalkingCor());
    }

    protected IEnumerator beginTalkingCor() {

        foreach (string message in messages) {
            yield return logController.TypeAnimation(message);
        }

        this.logController.ClearLogText();

        this.OnTalkingEnd();
    }

    protected virtual void OnTalkingEnd() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
        // TODO: give hero rewards
    }
}
