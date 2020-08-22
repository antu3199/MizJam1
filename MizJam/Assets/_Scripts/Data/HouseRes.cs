using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HouseRes", order = 1)]
public class HouseRes : ScriptableObject
{
    public List<GameObject> possibleNPCHouses;
    public List<GameObject> possibleEnemyHouses;

    public void InstantiateNPCHouse(Transform parent) {
        int index = Random.Range(0, this.possibleNPCHouses.Count);
        GameObject theObject = Instantiate(this.possibleNPCHouses[index], parent);
    }

    public void InstantiateEnemyHouse(Transform parent) {
        int index = Random.Range(0, this.possibleEnemyHouses.Count);
        GameObject theObject = Instantiate(this.possibleEnemyHouses[index], parent);
    }
}
