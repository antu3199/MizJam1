using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingMap : BasicMap
{
    // Start is called before the first frame update


    public List<LogMessage> messages;
    public List<Reward> rewards;

    public Transform rewardTransform;

    protected LogController logController;

    public override void DoInitialize() {
        this.logController = GameManager.Instance.gameController.logController;
    }

    public void BeginTalking() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
        StartCoroutine(this.beginTalkingCor());
    }

    protected IEnumerator beginTalkingCor() {

        foreach (LogMessage message in messages) {
            yield return logController.TypeAnimation(message);
        }

        yield return GameManager.Instance.gameController.InstantiateRewardCor(rewards, rewardTransform.position, rewardTransform);

        this.logController.ClearLogText();

        this.OnTalkingEnd();
    }

    protected virtual void OnTalkingEnd() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
        // TODO: give hero rewards
    }
}
