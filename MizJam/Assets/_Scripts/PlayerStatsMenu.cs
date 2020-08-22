using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsMenu : MonoBehaviour
{
    public Text attackText;
    public Text defenceText;
    public Text healthText;
    public Text ascensionAmountText;
    public Text ascensionBonusText;

    public void Initialize() {
        Messenger.AddListener<ResourceUpdate>(Messages.OnGPSUpdate, this.OnGPSUpdate);
        this.UpdateStats();
    }

    void OnDestroy() {
        Messenger.RemoveListener<ResourceUpdate>(Messages.OnGPSUpdate, this.OnGPSUpdate);
    }

    private void OnGPSUpdate(ResourceUpdate res) {
        this.UpdateStats();
    }

    private void UpdateStats() {
        PlayerStats playerStats = GameManager.Instance.gameState.playerStats;
        double reference = GameManager.Instance.gameState.GPS;

        this.attackText.text = "Attack: " + playerStats.GetScaledStat(Stat.ATTACK, reference);
        this.defenceText.text = "Defence: " + playerStats.GetScaledStat(Stat.DEFENCE, reference);
        this.healthText.text = "Health: " + playerStats.hp;
        this.ascensionAmountText.text = "Ascensions: " + GameManager.Instance.gameState.numAscensions;
        this.ascensionBonusText.text = "Ascension Bonus: " + GameManager.Instance.gameState.ascensionPoints + "%";
    }

}
