﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapScroller : MonoBehaviour
{

    public BasicMap currentMap;
    public float mapMoveSpeed = 1f;

    public void Initialize() {
        this.currentMap.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        this.currentMap.PerformUpdate();
    }
}