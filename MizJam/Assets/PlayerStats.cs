using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat {
    ATTACK,
    DEFENCE,
    MAX_HEALTH
};

public struct PlayerStat {
    public Stat stat;
    public double value;

    public PlayerStat(Stat stat, double value) {
        this.stat = stat;
        this.value = value;
    }
};


public class PlayerStats : MonoBehaviour
{

    [Header("Just for debugging")]
    public double hp = 1;
    private Dictionary<Stat, PlayerStat> stats = new Dictionary<Stat, PlayerStat>();

    public double tmp_ATTACK;
    public double tmp_DEFENCE;
    public double tmp_MAX_HEALTH;


    [Header("Modifiable Values")]
    public double attackGPSScaling = 0.1;
    public double defenceGPSScaling = 0.01;
    public double healthGPSScaling = 0.5;

    public void Initialize() {
        this.stats.Add(Stat.ATTACK, new PlayerStat(Stat.ATTACK, 0));
        this.stats.Add(Stat.DEFENCE, new PlayerStat(Stat.DEFENCE, 0));
        this.stats.Add(Stat.MAX_HEALTH, new PlayerStat(Stat.MAX_HEALTH, 0));
        this.hp = GetScaledStat(Stat.MAX_HEALTH);
    }

    public double GetScaledStat(Stat stat) {
        double GPS = GameManager.Instance.gameState.GPS;
        double result = 0;

        switch (stat) {
            case (Stat.ATTACK):
                result = GPS * (this.stats[Stat.ATTACK].value * attackGPSScaling);
                break;

            case (Stat.DEFENCE):
                result = GPS * (this.stats[Stat.DEFENCE].value * defenceGPSScaling);
                break;

            case (Stat.MAX_HEALTH):
                result = 1 + GPS * (this.stats[Stat.MAX_HEALTH].value * healthGPSScaling);
                break;
            default:
                Debug.LogError("ERROR: Stat not found");
                break;
        }

        return result;
    }


    public PlayerStat GetRawStat(Stat stat) {
        return this.stats[stat];
    }

    // Just for debugging 
    void Update() {
        this.tmp_ATTACK = GetScaledStat(Stat.ATTACK);
        this.tmp_DEFENCE = GetScaledStat(Stat.DEFENCE);
        this.tmp_MAX_HEALTH = GetScaledStat(Stat.MAX_HEALTH);
    }
}
