using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingMap : BasicMap
{

    public List<Reward> rewards;

    public Transform rewardTransform;
    public TutorialRes tutorialRes;
    public NPCSpriteRes npcSpriteRes;
    public TalkingMapRes talkingMapRes;
    public SpriteRenderer talkerSpriteRenderer;

    protected LogController logController;

    private List<string> messages;
    private Sprite logSprite;

    private int tutorialIndex = -1;

    public override void DoInitialize() {
        this.mapType = MapType.TALKING;
        Sprite gameSprite = this.npcSpriteRes.GetRandomGameSprite();
        this.talkerSpriteRenderer.sprite = gameSprite;
        this.levelIconSprite = gameSprite;
        this.logController = GameManager.Instance.gameController.logController;

        TalkingMapResObj talkingMapObj = this.talkingMapRes.GetRandomResObject();
        this.messages = talkingMapObj.messages;

        this.logSprite = this.npcSpriteRes.GetRandomLogSprite();

    }

    public void SetAsTutorial(int index) {
        this.tutorialIndex = index;
        this.mapType = MapType.TALKING_TUTORIAL;
        TutorialResObj tutorialObj = this.tutorialRes.tutorialObj[index];
        this.messages = tutorialObj.messages;
    }

    public void BeginTalking() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
        StartCoroutine(this.beginTalkingCor());
    }

    protected IEnumerator beginTalkingCor() {

        foreach (string message in this.messages) {
            yield return logController.TypeAnimation(new LogMessage(message, this.logSprite));
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
