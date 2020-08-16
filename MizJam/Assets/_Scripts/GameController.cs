using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public MapScroller mapScroller;

    public void Start() {

        GameManager.Instance.gameController = this;
        this.Initialize();
    }

    public void Initialize() {
        this.mapScroller.Initialize();
    }
}
