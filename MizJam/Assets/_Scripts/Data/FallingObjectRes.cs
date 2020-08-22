using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FallingObjectResObj {
    public Sprite fallingObjectSprite;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FallingObjectRes", order = 1)]
public class FallingObjectRes : ScriptableObject
{
    public List<FallingObjectResObj> possibleFallingObjects;

    public FallingObjectResObj GetRandomFallingObjectRes() {
        int index = Random.Range(0, this.possibleFallingObjects.Count);
        return this.possibleFallingObjects[index];
    }
}
