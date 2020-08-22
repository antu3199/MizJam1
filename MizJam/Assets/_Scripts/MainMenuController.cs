using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text playButtonText;
    void Start()
    {
        this.playButtonText.text = GameManager.Instance.gameState.hasPlayedTutorial ? "Continue" : "Play";
    }
}
