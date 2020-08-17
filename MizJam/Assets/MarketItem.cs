using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public Text titleText;
    public Text descText;
    public Image spriteIcon;
    public Text costText;
    public Image highlightBackground;

    public Item item{get; set;}

    public void Initialize(Item item) {
        this.item = item;
        titleText.text = item.CreationData.name;
        if (item.CreationData.icon != null) {
            spriteIcon.sprite = item.CreationData.icon;
        }

        this.UpdateCost();
        this.UpdateDescription();

        Messenger.AddListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
        // TODO: on buy, need to update cost, amount
    }

    public void OnGoldUpdate(ResourceUpdate update) {
        this.UpdateHighlighted();
    }

    public void UpdateHighlighted() {
        double currentGold = GameManager.Instance.gameState.gold;
        if (currentGold >= item.cost) {
            highlightBackground.gameObject.SetActive(true);
        } else {
            highlightBackground.gameObject.SetActive(false);
        }
    }

    public void UpdateCost() {
        this.costText.text = Currency.CurrencyToString(item.cost);
    }

    public void UpdateDescription() {
        this.descText.text = "+" + this.item.baseRate + " GPS"; 
    }

}
