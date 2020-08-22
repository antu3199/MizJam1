using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TradeResponse {
    YES,
    NO,
    NOT_ENOUGH_MONEY
};

public class TradeMap : BasicMap
{
    public Reward reward;
    public float goldCostMultiplierRange = 100.0f;

    public Transform rewardTransform;

    public Sprite logIcon;

    protected LogController logController;

    private const string messageBefore = "Welcome! We do gold trading here.";

    private const string askingMessageConst = "Would you like to trade {0} gold for {1}?";

    private const string responseYes = "Great! Have a nice day!";
    private const string responseNo = "Err... Are you sure?...";
    private const string responseNotEnoughMoney = "You don't have enough money, you dumbo";


    private bool responseReceived = false;
    private TradeResponse tradeResponse;

    private double goldCostToTrade = 0f;

    public override void DoInitialize() {
        this.logController = GameManager.Instance.gameController.logController;
    }

    public void BeginTalking() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
        StartCoroutine(this.beginTalkingCor());
    }

    public void PressAccept() {
        if (GameManager.Instance.gameState.gold >= this.goldCostToTrade) {
            this.tradeResponse = TradeResponse.YES;
        } else {
            this.tradeResponse = TradeResponse.NOT_ENOUGH_MONEY;
        }

        this.responseReceived = true;
    }

    public void PressDecline() {
        this.tradeResponse = TradeResponse.NO;
        this.responseReceived = true;
    }


    protected IEnumerator beginTalkingCor() {

        yield return logController.TypeAnimation(new LogMessage(messageBefore, this.logIcon));

        goldCostToTrade = GameManager.Instance.gameState.GPS  * Random.Range(0, goldCostMultiplierRange);
        double goldReceiveWhenTrade = GameManager.Instance.gameState.GPS  * Random.Range(goldCostMultiplierRange/2, goldCostMultiplierRange);


        string askingMessage = string.Format(askingMessageConst, Currency.CurrencyToString(goldCostToTrade), Currency.CurrencyToString(goldReceiveWhenTrade));
        
        yield return logController.TypeAnimation(new LogMessage(askingMessage, this.logIcon));

        GameManager.Instance.gameController.playerController.OverrideJumpButton(this.PressAccept, "Accept");
        GameManager.Instance.gameController.playerController.OverrideCancelButton(this.PressDecline, "Decline", true);

        // Wait for response
        while (!responseReceived) {
            yield return null;
        }

        GameManager.Instance.gameController.playerController.ResetOverrides();

        switch (this.tradeResponse) {
            case TradeResponse.YES:
                yield return logController.TypeAnimation(new LogMessage(responseYes, this.logIcon));
                GameManager.Instance.gameState.AddGold(-goldCostToTrade);
                yield return GameManager.Instance.gameController.InstantiateRewardCor(reward, rewardTransform.position, rewardTransform);

                break;

            case TradeResponse.NO:
                yield return logController.TypeAnimation(new LogMessage(responseNo, this.logIcon));
                break;

            case TradeResponse.NOT_ENOUGH_MONEY:
                yield return logController.TypeAnimation(new LogMessage(responseNotEnoughMoney, this.logIcon));
                break;
        }

        this.logController.ClearLogText();

        this.OnTalkingEnd();
    }

    protected virtual void OnTalkingEnd() {
        GameManager.Instance.gameController.mapScroller.SetMapMovable(true);
        this.CallGoToNextMap();
    }
}
