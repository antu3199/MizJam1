using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public MarketItem marketItemPrefab;

    public Transform marketItemTransform;

    private List<MarketItem> marketItems = new List<MarketItem>();

    // Start is called before the first frame update
    void Start()
    {
        List<Item> items = GameManager.Instance.gameState.items;
        for (int i = 0; i < items.Count; i++) {
            MarketItem marketItem = Instantiate(marketItemPrefab, marketItemTransform) as MarketItem;
            marketItem.Initialize(items[i]);
        }

        // TODO: Only show items up to owned+1 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
