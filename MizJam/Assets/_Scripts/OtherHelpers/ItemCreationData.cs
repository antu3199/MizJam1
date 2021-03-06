﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCreationData
{
   public string name;
   public string description;
   public List<Sprite> icons;

   public Stat statToIncrease;
   public double statIncreaseAmount;

   public int DEBUG_initialOwned = 0;
}
