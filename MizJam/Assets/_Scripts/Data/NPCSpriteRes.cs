using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NPCSpriteRes", order = 1)]
public class NPCSpriteRes : ScriptableObject
{
    public List<Sprite> possibleGameSprites;

    public List<Sprite> possibleLogSprites;

    public Sprite GetRandomGameSprite() {
        int index = Random.Range(0, this.possibleGameSprites.Count);
        return this.possibleGameSprites[index];
    }

    public Sprite GetRandomLogSprite() {
        int index = Random.Range(0, this.possibleLogSprites.Count);
        return this.possibleLogSprites[index];
    }

}
