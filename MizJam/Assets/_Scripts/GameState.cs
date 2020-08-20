﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : PersistableObject
{
    public double GPS = 0;
    public PlayerStats playerStats;

    public void Initialize() {
        playerStats.Initialize();
    }

    public void AfterLoad() {
        this.UpdateGPS();
    }

    public void UpdateGPS(bool ignoreBroadcastIfNoChange = true) {
        double oldGPS = this.GPS;
        GPS = 0;
        foreach (Item item in this.items) {
            GPS += item.GetGoldProduction(); 
        }

        if (!ignoreBroadcastIfNoChange || oldGPS != GPS) {
            Messenger.Broadcast<ResourceUpdate>(Messages.OnGPSUpdate, new ResourceUpdate(Resource.GPS, GPS, this.GPS));
        }
    }

    public void SetLevel(int level) {
        this.floorNumber = level;
        GameManager.Instance.gameController.topBar.UpdateLevelText();
    }

    // Saveables ===
    [Header("Savables")]
    public double gold;
    public List<Item> items;
    public int floorNumber = 0;

    // ===

    public void AddGold(double goldAmount) {
        double oldGold = this.gold;
        this.gold += goldAmount;

        Messenger.Broadcast<ResourceUpdate>(Messages.OnGoldUpdate, new ResourceUpdate(Resource.GOLD, oldGold, this.gold));
    }
    
	public override void Save (GameDataWriter writer) {
        writer.Write(gold);
        writer.Write(floorNumber);
        writer.Write(items.Count);
        for (int i = 0; i < items.Count; i++) {
            items[i].Save(writer);
        }
	}

	public override void Load (GameDataReader reader) {
        this.gold = reader.ReadDouble();
        this.floorNumber = reader.ReadInt();
        int itemsCount = reader.ReadInt();
        for (int i = 0; i < items.Count; i++) {
            items[i].Load(reader);
        }
	}
}