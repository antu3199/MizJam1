using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AscensionMenu : MonoBehaviour
{

    public Text bonusIfAscendText;
    public Text dontHaveEnoughText;
    public Button ascendButton;
    

    public void Initialize() {
        Messenger.AddListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
        this.UpdateValues();
    }

    void OnDestroy() {
        Messenger.RemoveListener<ResourceUpdate>(Messages.OnGoldUpdate, this.OnGoldUpdate);
    }

    private void OnGoldUpdate(ResourceUpdate res) {
        this.UpdateValues();
    }

    private void UpdateValues() {
        double curGold = GameManager.Instance.gameState.gold;
        double ascensionPointsIfAscend = Currency.GetNumAscendPoints(curGold);
        this.bonusIfAscendText.text = "Bonus if you ascend: " + ascensionPointsIfAscend + "%";

        bool canAscend = (ascensionPointsIfAscend >= 1.0);
        this.dontHaveEnoughText.gameObject.SetActive(!canAscend);
        this.ascendButton.interactable = canAscend;
    }

    public void Ascend() {
        double curGold = GameManager.Instance.gameState.gold;
        double ascensionPointsIfAscend = Currency.GetNumAscendPoints(curGold);
        GameManager.Instance.gameState.numAscensions++;
        GameManager.Instance.gameState.ascensionPoints += ascensionPointsIfAscend;
        this.ResetProgress();

        SceneManager.LoadScene("MainGame");
    }

    private void ResetProgress() {
        GameManager.Instance.gameState.gold = 0;

        foreach(Item item in GameManager.Instance.gameState.items) {
            item.owned = 0;
        }
        GameManager.Instance.gameState.floorNumber = 0;
        GameManager.Instance.gameStorage.SaveGame();
    }

}
