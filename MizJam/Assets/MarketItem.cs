﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public Text titleText;
    public Text descText;
    public Image spriteIcon;
    public Text costText;

    public Button buyButton;
    public Image highlightBackground;

    public Item item{get; set;}

    public void Initialize(Item item) {
        this.item = item;
        this.UpdateTitle();
        if (item.CreationData.icons[0] != null) {
            spriteIcon.sprite = item.CreationData.icons[0];
        }

        this.UpdateCost();
        this.UpdateDescription();

        Messenger.AddListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
        // TODO: on buy, need to update cost, amount
    }

    public void UpdateAll() {
        this.UpdateCost();
        this.UpdateTitle();
        this.UpdateDescription();
        this.UpdateHighlighted();
        this.UpdateSprite();
    }


    public void OnGoldUpdate(ResourceUpdate update) {
        this.UpdateHighlighted();
    }

    public void UpdateHighlighted() {
        double currentGold = GameManager.Instance.gameState.gold;
        if (currentGold >= item.cost) {
            highlightBackground.gameObject.SetActive(true);
            buyButton.interactable = true;
        } else {
            highlightBackground.gameObject.SetActive(false);
            buyButton.interactable = false;
        }
    }

    public void UpdateTitle() {
        string title = item.CreationData.name;
        if (item.owned >= 1) {
            title += " *" + item.owned;
        }

        this.titleText.text = title;
    }

    public void UpdateCost() {
        this.costText.text = Currency.CurrencyToString(item.cost);
    }

    public void UpdateDescription() {
        this.descText.text = "+" + Currency.CurrencyToString(this.item.baseRate) + " GPS"; 
    }

    public void UpdateSprite() {
        this.spriteIcon.sprite = this.item.GetSprite();
    }

    public void BuyItem() {
        if (GameManager.Instance.gameState.gold >= item.cost) {
            item.SetOwned(item.owned + 1);
            GameManager.Instance.gameState.AddGold(-item.cost);
            this.UpdateAll();
            GameManager.Instance.gameStorage.SaveGame();
        }
    }
}