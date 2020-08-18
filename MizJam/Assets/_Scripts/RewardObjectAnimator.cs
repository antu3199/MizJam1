using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardObjectAnimator : MonoBehaviour
{

    public CanvasGroup content;
    public float timeToDisappear = 2f;

    private float timeCounter = 0;

    public float moveUpSpeed = 0.1f;

    public bool useVariance = false;
    public float variance = 0.02f;

    public Image iconImage;
    public Text contentText;

    private Reward reward;


    public void Initialize(Reward reward) {
        this.reward = reward;
        this.iconImage.sprite = reward.icon;
        this.iconImage.color = reward.iconColor;
        this.SetText();

        if (useVariance) {
            float varianceX = Random.Range(-variance, variance);
            float varianceY = Random.Range(-variance, variance);

            this.gameObject.transform.position = new Vector3(this.transform.position.x + varianceX, this.transform.position.y + varianceY, this.transform.position.z);
        }
    }

    private void SetText() {

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

    void Update() {
        this.transform.position += new Vector3(0, moveUpSpeed * Time.deltaTime, 0); 

        timeCounter += Time.deltaTime;

        float t = timeCounter / timeToDisappear;
        content.alpha = 1.0f-t;

        if (timeCounter >= timeToDisappear) {
            this.timeToDisappear = 0;
            Destroy(this.gameObject);
        }
    }
}
