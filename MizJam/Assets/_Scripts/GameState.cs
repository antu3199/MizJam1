using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : PersistableObject
{
    public double GPS = 0;
    public PlayerStats playerStats;

    public void Initialize() {
        playerStats.Initialize();
        playerStats.UpdateMaxHealth(this.GPS);
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

        double percentageModifiers = 0;
        percentageModifiers += this.ascensionPoints;
        GPS = GPS + (GPS * percentageModifiers);

        if (!ignoreBroadcastIfNoChange || oldGPS != GPS) {
            Messenger.Broadcast<ResourceUpdate>(Messages.OnGPSUpdate, new ResourceUpdate(Resource.GPS, GPS, this.GPS));
        }

        playerStats.UpdateMaxHealth(this.GPS);

        this.UpdatePlayerStats();
    }

    public void UpdatePlayerStats() {
        this.playerStats.SetStatsToBase();

        PlayerStat attackStat = this.playerStats.GetRawStat(Stat.ATTACK);
        PlayerStat defenceStat = this.playerStats.GetRawStat(Stat.DEFENCE);
        PlayerStat maxHealthStat = this.playerStats.GetRawStat(Stat.MAX_HEALTH);

        foreach (Item item in this.items) {
            double value = item.CreationData.statIncreaseAmount * item.owned;
            
            switch (item.CreationData.statToIncrease) {
                case Stat.ATTACK:
                    attackStat.value += value;
                    break;
                case Stat.DEFENCE:
                    defenceStat.value += value;
                    break;
                case Stat.MAX_HEALTH:
                    maxHealthStat.value += value;
                    break;
            }
        }

        Messenger.Broadcast(Messages.OnStatsUpdate);
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
    public bool hasPlayedTutorial = false;
    public double ascensionPoints = 0;
    public int numAscensions = 0;

    public double lastSaveTime = 0;

    // ===

    public void AddGold(double goldAmount) {
        double oldGold = this.gold;
        this.gold += goldAmount;

        Messenger.Broadcast<ResourceUpdate>(Messages.OnGoldUpdate, new ResourceUpdate(Resource.GOLD, oldGold, this.gold));
    }
    
	public override void Save (GameDataWriter writer) {

        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        double secondsSinceEpoch = t.TotalSeconds;
        this.lastSaveTime = secondsSinceEpoch;

        writer.Write(gold);
        writer.Write(floorNumber);
        writer.Write(items.Count);
        for (int i = 0; i < items.Count; i++) {
            items[i].Save(writer);
        }

        writer.Write(hasPlayedTutorial);
        writer.Write(ascensionPoints);
        writer.Write(numAscensions);

        writer.Write(lastSaveTime);

	}

	public override void Load (GameDataReader reader) {
        this.gold = reader.ReadDouble();
        this.floorNumber = reader.ReadInt();
        int itemsCount = reader.ReadInt();
        for (int i = 0; i < items.Count; i++) {
            items[i].Load(reader);
        }

        this.hasPlayedTutorial = reader.ReadBool();
        this.ascensionPoints = reader.ReadDouble();
        this.numAscensions = reader.ReadInt();

        this.lastSaveTime = reader.ReadDouble();
        if (this.lastSaveTime != 0) {
            StartCoroutine(this.GiveMoneyWhenControllerReady());
        }
	}

    private IEnumerator GiveMoneyWhenControllerReady() {
        while (GameManager.Instance.gameController == null) {
            yield return null;
        }

        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        double secondsSinceEpoch = t.TotalSeconds;


        double secondsSinceLastAway = secondsSinceEpoch - lastSaveTime;
        Debug.Log("Last epoch: " + secondsSinceEpoch + " this epoch: " + secondsSinceEpoch + " Delta: " + secondsSinceLastAway);

        GameManager.Instance.gameState.UpdateGPS();
        double addedGold = secondsSinceLastAway * GameManager.Instance.gameState.GPS;
        GameManager.Instance.gameState.AddGold( addedGold );
        LogMessage message = new LogMessage("You have gained: " + Currency.CurrencyToString(addedGold) + " gold while you were away!", null);
        yield return GameManager.Instance.gameController.logController.TypeAnimation(message);
    }
}
