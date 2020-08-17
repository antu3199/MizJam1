using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : PersistableClass
{
    public int owned; // This is probably the only thing that needs to get saved

    public double cost;

    public double multiplier;

    public double baseCost;
    public double baseRate;

    public int Index;
    public ItemCreationData CreationData;

    public Item(int itemIndex, ItemCreationData creationData, int owned = 0) {
        this.Index = itemIndex;
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
    }

    public void UpdateMultiplier(double multiplier) {
        this.multiplier = multiplier;
    }

    public double GetGoldProduction() {
        return  Currency.GetGoldProduction(this.baseRate, this.owned,this.multiplier);
    }

	public override void Save (GameDataWriter writer) {
		writer.Write(owned);
	}

	public override void Load (GameDataReader reader) {
		this.owned = reader.ReadInt();
	}

}
