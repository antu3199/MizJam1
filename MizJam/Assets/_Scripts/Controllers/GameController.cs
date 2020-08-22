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


    public void Start() {

        GameManager.Instance.gameController = this;
        this.Initialize();
    }

    public void Initialize() {
        this.topBar.Initialize();
        this.mapScroller.Initialize();
        this.InstantiatePlayer();
    }

    public void Update() {

        if (Input.GetKeyDown("1")) {
            GameManager.Instance.gameStorage.SaveGame();
        }

        if (Input.GetKeyDown("2")) {
            GameManager.Instance.gameStorage.DeleteSaveFile();
        }

        if (Input.GetKeyDown("0")) {
            //this.RestartRun();
            SceneManager.LoadScene("MainGame");
        }

        if (Input.GetKeyDown("9")) {
            GameManager.Instance.gameState.AddGold( System.Math.Pow(10, 12));
        }

        GameManager.Instance.gameState.AddGold(GameManager.Instance.gameState.GPS * Time.deltaTime);

        GameManager.Instance.gameStorage.timeSinceLastSave += Time.deltaTime;
        if (GameManager.Instance.gameStorage.timeSinceLastSave >= GameManager.Instance.gameStorage.autoSaveTime) {
            GameManager.Instance.gameStorage.SaveGame();
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
        yield return this.logController.TypeAnimation(logMessage);

        yield return new WaitForSeconds(0.5f);
        this.ResetGame();
        //SceneManager.LoadScene("MainGame");
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
