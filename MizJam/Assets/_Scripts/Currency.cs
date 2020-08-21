using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Currency
{
    private static string[] suffix = new string[]{"","k","M","G","T","P","E"}; // kilo, mega, giga, terra, penta, exa
    // Source: https://answers.unity.com/questions/1234587/idle-game-currency-converter.html
        
    public static string CurrencyToString(double valueToConvert) {
        int scale = 0;
        double v = valueToConvert;
        while(v >= 1000d)
        {
            v /= 1000d;
            scale++;
            if (scale >= suffix.Length)
                return valueToConvert.ToString("e2"); // overflow, can't display number, fallback to exponential
        }
        return v.ToString("0.###") + suffix[scale];
     }


    // Source: https://gameanalytics.com/blog/idle-game-mathematics.html
    private const double MULTIPLIER = 1.07; // Random constant between 1.08 and 1.15
    private const int BASE_COST_MULTIPLIER = 10;
    private const int BASE_RATE_MULTIPLIER = 30;
        

    // Used for setup 
    public static double GetBaseCost(int itemNumber) {

        return GetBaseRate(itemNumber) * (7 + itemNumber * 1.5);
       // return Math.Pow(MULTIPLIER, BASE_COST_MULTIPLIER * itemNumber);
    }

    public static double GetBaseRate(int itemNumber) {
        return Math.Pow(MULTIPLIER, BASE_RATE_MULTIPLIER * itemNumber);
    }

    // Helpers for function

    // baseCost obtained from GetBaseCost()
    public static double GetCostToUpgrade(double baseCost, float numOwned) {
        return baseCost * Math.Pow(MULTIPLIER, numOwned);
    }

    // baseRate is obtained from GetBaseRate()
    public static double GetGoldProduction(double baseRate, int owned, double multipliers) {
        return (baseRate * owned) * multipliers;
    }

    public static double GetEnemyScaling(int index) {
        return GetBaseRate(index) / 2;
    }

    public static double GetNumAscendPoints(double goldValue) {
        return Math.Sqrt( goldValue / Math.Pow(10, 11));
    }
}
