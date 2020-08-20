using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenu : MonoBehaviour
{
    public Market market;
    public Text goldText;

    public Text GPSText;


    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
        Messenger.AddListener<ResourceUpdate>(Messages.OnGPSUpdate, this.OnGPSUpdate);

        // Force update
        GameManager.Instance.gameState.AddGold(0);
        GameManager.Instance.gameState.UpdateGPS(false);
    }

    void OnDestroy() {
        Messenger.RemoveListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
        Messenger.RemoveListener<ResourceUpdate>(Messages.OnGPSUpdate, this.OnGPSUpdate);
    }

    public void OnGoldUpdate(ResourceUpdate update) {
        this.goldText.text = Currency.CurrencyToString(update.valueAfter);
    }

    public void OnGPSUpdate(ResourceUpdate update) {
        this.GPSText.text = Currency.CurrencyToString(update.valueAfter) + " GPS";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
