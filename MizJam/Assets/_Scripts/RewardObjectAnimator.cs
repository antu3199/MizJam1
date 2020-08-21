using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardObjectAnimator : MonoBehaviour
{

    public Image iconImage;
    public Text contentText;

    private Reward reward;

    public void Initialize(Reward reward) {
        this.reward = reward;
        this.iconImage.sprite = reward.icon;
        this.iconImage.color = reward.iconColor;
        this.GiveReward();
    }

    private void GiveReward() {

        switch (this.reward.rewardType) {
            case RewardType.GOLD:
                double goldGain = 10 + GameManager.Instance.gameState.GPS * this.reward.value;

                GameManager.Instance.gameState.AddGold(goldGain);

                this.contentText.text = Currency.CurrencyToString(goldGain);
                break;

            case RewardType.MODIFIER:
                // TODO later...
                break;

            case RewardType.ITEM:
                GameManager.Instance.gameState.items[reward.indexIfItem].SetOwned(GameManager.Instance.gameState.items[reward.indexIfItem].owned + (int)reward.value);
                this.contentText.text = "+" + reward.value.ToString();
                break;
     


        }
    }
}
