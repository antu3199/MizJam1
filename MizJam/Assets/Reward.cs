﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType {
    GOLD,
    MODIFIER,
    ITEM

};

[System.Serializable]
public class Reward 
{
    public double value;
    public int indexIfItem = 0;
    public Sprite icon;
    public Color iconColor;

    public RewardType rewardType;


}