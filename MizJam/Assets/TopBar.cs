using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public Text levelText;

    public void UpdateLevelText() {
        this.levelText.text = string.Format("Miles travelled {0}", GameManager.Instance.gameState.floorNumber);
    }
}
