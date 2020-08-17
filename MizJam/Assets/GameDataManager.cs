using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains information regarding high-level data such as items or upgrades, but doesn't necessarily control them.

public class GameDataManager : MonoBehaviour {
    public List<ItemCreationData> marketItems;

    public void Initialize() {
        GameManager.Instance.gameState.items = new List<Item>();
        for (int i = 0; i < marketItems.Count; i++) {
            ItemCreationData creationData = marketItems[i];
            GameManager.Instance.gameState.items.Add(new Item(i, creationData));
        }
    }

}
