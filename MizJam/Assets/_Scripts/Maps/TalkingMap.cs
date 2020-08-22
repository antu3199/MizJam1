using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingMap : BasicMap
{

    public List<LogMessage> messages;
    public List<Reward> rewards;

    public Transform rewardTransform;
    public TutorialRes tutorialRes;

    protected LogController logController;

    private int tutorialIndex = -1;

    public override void DoInitialize() {
        this.mapType = MapType.TALKING;
        this.logController = GameManager.Instance.gameController.logController;
    }

    public void SetAsTutorial(int index) {
        this.tutorialIndex = index;
        this.mapType = MapType.TALKING_TUTORIAL;
        TutorialResObj tutorialObj = this.tutorialRes.tutorialObj[index];
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
        if (tutorialIndex == 3 && this.mapType == MapType.TALKING_TUTORIAL) {
            GameManager.Instance.gameState.hasPlayedTutorial = true;
            GameManager.Instance.gameStorage.SaveGame();
        }

        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
        this.CallGoToNextMap();
    }
}
