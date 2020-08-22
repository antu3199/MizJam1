using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat {
    ATTACK,
    DEFENCE,
    MAX_HEALTH,
    OTHER
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

    public double baseAttack = 1;
    public double baseDefence = 0;
    public double baseMaxHealth = 1;


    [Header("Modifiable Values")]
    public double attackScaling = 0.1;
    public double defenceScaling = 0.01;
    public double healthScaling = 0.5;

    public void Initialize() {
        this.stats.Add(Stat.ATTACK, new PlayerStat(Stat.ATTACK, baseAttack));
        this.stats.Add(Stat.DEFENCE, new PlayerStat(Stat.DEFENCE, baseDefence));
        this.stats.Add(Stat.MAX_HEALTH, new PlayerStat(Stat.MAX_HEALTH, baseMaxHealth));
        this.hp = GetScaledStat(Stat.MAX_HEALTH, 0);
    }

    public void UpdateMaxHealth(double reference) {
        double newHealth = this.GetScaledStat(Stat.MAX_HEALTH, reference);
        double delta = newHealth - this.hp;
        this.hp += delta;
        this.hp = System.Math.Min(this.hp, newHealth);
    }

    public virtual double GetScaledStat(Stat stat, double reference) {
        double result = 0;

        switch (stat) {
            case (Stat.ATTACK):
                result = 1 + reference * (this.stats[Stat.ATTACK].value ) * attackScaling;
                break;

            case (Stat.DEFENCE):
                result = reference * ( this.stats[Stat.DEFENCE].value ) * defenceScaling;
                break;

            case (Stat.MAX_HEALTH):
                result = 1 + reference * (this.stats[Stat.MAX_HEALTH].value ) * healthScaling;
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

    // Note: Damage should be obtained through GetScaledStat
    public double DealDamageToMe(double damage, double reference) {
        double defenceStat = GetScaledStat(Stat.DEFENCE, reference);
        double healthStat = GetScaledStat(Stat.MAX_HEALTH, reference);

        double finalDamage = damage - defenceStat;
        if (finalDamage < 0) {
            finalDamage = 0;
        }

        this.hp -= finalDamage;
        return finalDamage;
    }

    public void SetStatsToBase() {
        PlayerStat attackStat = this.stats[Stat.ATTACK];
        PlayerStat defenceStat = this.stats[Stat.DEFENCE];
        PlayerStat healthStat = this.stats[Stat.MAX_HEALTH];

        attackStat.value = this.baseAttack;
        defenceStat.value = this.baseDefence;
        healthStat.value = this.baseMaxHealth;
    }

    public void SetValues(double baseAttack, double attackScaling, double baseDefence, double defenceScaling, double baseHealth, double healthScaling) {
        this.attackScaling = attackScaling;
        this.defenceScaling = defenceScaling;
        this.healthScaling = healthScaling;

        PlayerStat attackStat = this.stats[Stat.ATTACK];
        PlayerStat defenceStat = this.stats[Stat.DEFENCE];
        PlayerStat healthStat = this.stats[Stat.MAX_HEALTH];

        attackStat.value = baseAttack;
        defenceStat.value = baseDefence;
        healthStat.value = baseHealth;
    }

    // Just for debugging 
    void Update() {
        this.tmp_ATTACK = GetScaledStat(Stat.ATTACK, GameManager.Instance.gameState.GPS);
        this.tmp_DEFENCE = GetScaledStat(Stat.DEFENCE, GameManager.Instance.gameState.GPS);
        this.tmp_MAX_HEALTH = GetScaledStat(Stat.MAX_HEALTH, GameManager.Instance.gameState.GPS);
    }

    public static string StatToString(Stat stat) {
        switch (stat) {
            case Stat.ATTACK:
                return "Attack";
            case Stat.DEFENCE:
                return "Defence";
            case Stat.MAX_HEALTH:
                return "Max Health";
        }

        return "";
    }
}
