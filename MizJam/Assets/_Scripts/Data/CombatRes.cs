using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CombatResObj {


    // public Sprite logSprite; // For simplicity, just make it same as gameSprite
    public Sprite gameSprite;


    [Header("Enemy Stats")]
    public double baseAttack = 1;
    public double baseDefence = 0;
    public double baseMaxHealth = 1;

    public double attackScaling = 0.1;
    public double defenceScaling = 0.01;
    public double healthScaling = 0.5;

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CombatRes", order = 1)]
public class CombatRes : ScriptableObject
{
    [TextArea]
    public List<string> possibleMessages;

    public List<CombatResObj> possibleCombatObj;

    public CombatResObj GetRandomResObject() {
        int index = Random.Range(0, possibleCombatObj.Count);
        return this.possibleCombatObj[index];
    }

    public string GetRandomMessage() {
        int index = Random.Range(0, this.possibleMessages.Count);
        return this.possibleMessages[index];
    }


}
