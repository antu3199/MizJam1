using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingMap : BasicMap
{
    // Start is called before the first frame update

    const float REWARD_RECEIVE_DELAY = 0.5f;

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

        foreach (Reward reward in rewards) {
            GameManager.Instance.gameController.InstantiateReward(reward, rewardTransform.position, rewardTransform);
            yield return new WaitForSeconds(REWARD_RECEIVE_DELAY);
        }

        this.logController.ClearLogText();

        this.OnTalkingEnd();
    }

    protected virtual void OnTalkingEnd() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
        // TODO: give hero rewards
    }
}
