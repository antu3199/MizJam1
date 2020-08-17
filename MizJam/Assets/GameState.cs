using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : PersistableObject
{
    public double CPS = 0;

    // Saveables ===
    public double Coins;
    public List<Item> items;

    // ===

    public void Initialize() {
        
    }

    public void AfterLoad() {
        this.UpdateCPS();
    }

    public void UpdateCPS() {
        CPS = 0;
        foreach (Item item in this.items) {
            CPS += item.GetGoldProduction(); 
        }
    }

    public void AddCoins(double coinAmount) {
        this.Coins += coinAmount;
    }
    
	public override void Save (GameDataWriter writer) {
        writer.Write(Coins);
        writer.Write(items.Count);
        for (int i = 0; i < items.Count; i++) {
            items[i].Save(writer);
        }
	}

	public override void Load (GameDataReader reader) {
        int itemsCount = reader.ReadInt();
        for (int i = 0; i < items.Count; i++) {
            items[i].Load(reader);
        }
	}
}
