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
            this.marketItems.Add(marketItem);
        }

        Messenger.AddListener<ItemUpdate>(Messages.OnItemBuy, this.OnItemBuy);

        this.UpdateShownItems();
        // TODO: Only show items up to owned+1 
    }

    private void OnItemBuy(ItemUpdate message) {
        marketItems[message.itemIndex].UpdateAll();
        this.UpdateShownItems();
    }


    public void UpdateShownItems() {
        int indexToShow = -1;
        for (int i = 0; i < marketItems.Count; i++) {
            if (marketItems[i].item.owned == 0) {
                indexToShow = i;
                break;
            }
        }

        if (indexToShow == -1) {
            indexToShow = marketItems.Count-1;
        }
        

        for (int i = 0; i <= indexToShow; i++) {
            marketItems[i].gameObject.SetActive(true);
        }

        for (int i = indexToShow+1; i < marketItems.Count; i++) {
            marketItems[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
