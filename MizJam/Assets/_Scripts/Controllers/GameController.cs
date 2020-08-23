using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    const float REWARD_RECEIVE_DELAY = 0.5f;
    
    public PlayerController playerPrefab;

    public MapScroller mapScroller;
    public LogController logController;
    public RewardObjectAnimator rewardObjectAnimatorPrefab;
    public TopBar topBar;

    public PlayerController playerController{get; set;}

    public BasicText damageTextPrefab;

    public CanvasGroup fadeInImage;


    public void Start() {

        GameManager.Instance.gameController = this;
        this.Initialize();
    }

    public void Initialize() {
        this.fadeInImage.alpha = 0;
        this.topBar.Initialize();
        this.mapScroller.Initialize();
        this.InstantiatePlayer();
    }

    public void Update() {

        // Test functions (I think it's okay to leave these here for the final build...)
        if (Input.GetKeyDown("1")) {
            GameManager.Instance.gameStorage.SaveGame();
        }

        if (Input.GetKeyDown("2")) {
            GameManager.Instance.gameStorage.DeleteSaveFile();
        }

        if (Input.GetKeyDown("0")) {
            SceneManager.LoadScene("MainGame");
        }

        // Not this though
        if (Input.GetKeyDown("9")) {
            //GameManager.Instance.gameState.AddGold( System.Math.Pow(10, 12));
        }

        GameManager.Instance.gameState.AddGold(GameManager.Instance.gameState.GPS * Time.deltaTime);

        GameManager.Instance.gameStorage.timeSinceLastSave += Time.deltaTime;
        if (GameManager.Instance.gameStorage.timeSinceLastSave >= GameManager.Instance.gameStorage.autoSaveTime) {
            GameManager.Instance.gameStorage.SaveGame(false);
        }

    }

    public IEnumerator InstantiateRewardCor(List<Reward> rewards, Vector3 position, Transform parent = null) {
        foreach (Reward reward in rewards) {
            this.InstantiateReward(reward, position, parent);
            yield return new WaitForSeconds(REWARD_RECEIVE_DELAY);
        }
    }

    public IEnumerator InstantiateRewardCor(Reward reward, Vector3 position, Transform parent = null) {
        this.InstantiateReward(reward, position, parent);
        yield return new WaitForSeconds(REWARD_RECEIVE_DELAY);
    }

    // Call when death
    public void OnPlayerDeath() {
        this.mapScroller.SetMapMovable(false);
        this.StartCoroutine(this.RestartRun());
    }

    public void InstantiateDamageText(double damage, bool isPlayer, Vector3 position, Transform parent = null) {
        BasicText textObject = Instantiate(this.damageTextPrefab, parent) as BasicText;
        textObject.transform.position = position;
        textObject.text.text = Currency.CurrencyToString(damage);
        textObject.text.color = isPlayer ? textObject.playerColor : textObject.enemyColor; 
    }

    public void ResetGame() {
        this.logController.ClearLogText();
        this.mapScroller.Reset();
        Destroy(this.playerController.gameObject);
        this.InstantiatePlayer();
    }

    private IEnumerator RestartRun() {
        string message = "Your vision blurs...";
        LogMessage logMessage = new LogMessage(message, null);
        this.StartCoroutine(this.logController.TypeAnimation(logMessage));

        float counter = 0;
        float fadeTime = 2f;

        while (counter < fadeTime) {
            float t = counter / fadeTime;
            this.fadeInImage.alpha = t;
            counter += Time.deltaTime;
            yield return null;
        }

        this.fadeInImage.alpha = 1;

        this.ResetGame();
        counter = 0;

        while (counter < fadeTime) {
            float t = counter / fadeTime;
            this.fadeInImage.alpha = 1 - t;
            counter += Time.deltaTime;
            yield return null;
        }

        this.fadeInImage.alpha = 0;
    }

    private void InstantiatePlayer() {
        PlayerController thePlayer = Instantiate(playerPrefab) as PlayerController;
        thePlayer.transform.position = mapScroller.playerDefaultPosition.position;
        this.playerController = thePlayer;
        thePlayer.Initialize();
    }


    private void InstantiateReward(Reward reward, Vector3 position, Transform parent = null) {
        RewardObjectAnimator rewardObject = Instantiate(this.rewardObjectAnimatorPrefab, parent) as RewardObjectAnimator;
        rewardObject.transform.position = position;
        rewardObject.Initialize(reward);
    }
}
