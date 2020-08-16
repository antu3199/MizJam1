using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    
    public MapScroller mapScroller;
    public LogController logController;


    public void Start() {

        GameManager.Instance.gameController = this;
        this.Initialize();
    }

    public void Initialize() {
        this.mapScroller.Initialize();
    }
}
