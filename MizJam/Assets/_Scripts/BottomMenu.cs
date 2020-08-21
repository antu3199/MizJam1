using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenu : MonoBehaviour
{
    public Market market;
    public PlayerStatsMenu playerStatsMenu;
    public AscensionMenu ascensionMenu;

    public Text goldText;

    public Text GPSText;


    public List<Transform> tabs;

    public List<ImageIcon> tabIcons;
    
    private int curTabOpen = 0;

    public Text titleText;


    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
        Messenger.AddListener<ResourceUpdate>(Messages.OnGPSUpdate, this.OnGPSUpdate);

        // Force update
        GameManager.Instance.gameState.AddGold(0);
        GameManager.Instance.gameState.UpdateGPS(false);

        this.market.Initialize();
        this.playerStatsMenu.Initialize();
        this.ascensionMenu.Initialize();
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

    public void SwitchTab(int index) {
        if (index == this.curTabOpen) {
            return;
        }

        this.tabs[this.curTabOpen].gameObject.SetActive(false);
        this.tabIcons[this.curTabOpen].SetHighlight(false);
        this.curTabOpen = index;
        this.tabs[this.curTabOpen].gameObject.SetActive(true);
        this.tabIcons[this.curTabOpen].SetHighlight(true);

        switch (index) {
            case 0:
                this.titleText.text = "Market";
            break;
            case 1:
                this.titleText.text = "Stats";
                break;
            
            case 2:
                this.titleText.text = "Ascend";
                break;
            default:
                break;
        }
    }

    
}
