using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemBoughtMessage {
    public Item item;
    public ItemBoughtMessage(Item item) {
        this.item = item;
    }
};

[System.Serializable]
public class Item : PersistableClass
{
    public int owned; // This is probably the only thing that needs to get saved

    public double cost;

    public double multiplier;

    public double baseCost;
    public double baseRate;

    public int index;
    public ItemCreationData CreationData;

    public Item(int itemIndex, ItemCreationData creationData, int owned = 0) {
        this.index = itemIndex;
        this.CreationData = creationData;

        this.baseCost = Currency.GetBaseCost(itemIndex);
        this.baseRate = Currency.GetBaseRate(itemIndex);
        this.owned = owned;
        this.cost = Currency.GetCostToUpgrade(baseCost, owned);
        this.multiplier = 1;
    }

    public void SetOwned(int newOwned) {
        this.owned = newOwned;
        this.cost = Currency.GetCostToUpgrade(baseCost, owned);
        GameManager.Instance.gameState.UpdateGPS();
        Messenger.Broadcast<ItemBoughtMessage>(Messages.OnItemBuy, new ItemBoughtMessage(this));
    }


    public void UpdateMultiplier(double multiplier) {
        this.multiplier = multiplier;
    }

    public double GetGoldProduction() {
        return  Currency.GetGoldProduction(this.baseRate, this.owned,this.multiplier);
    }

    public Sprite GetSprite() {
        Sprite theSprite = null;

        if (this.owned >= 1000) {
            theSprite = this.CreationData.icons[3];
        } else if (this.owned >= 100) {
            theSprite = this.CreationData.icons[2];
        } else if (this.owned >= 10) {
            theSprite = this.CreationData.icons[1];
        } else {
            theSprite = this.CreationData.icons[0];
        }

        return theSprite;
    }


	public override void Save (GameDataWriter writer) {
		writer.Write(owned);
	}

	public override void Load (GameDataReader reader) {
		this.owned = reader.ReadInt();
	}

}
