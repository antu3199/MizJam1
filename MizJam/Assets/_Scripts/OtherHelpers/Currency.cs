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
    // https://cookieclicker.fandom.com/wiki/Building
    private const double MULTIPLIER = 1.07; // Random constant between 1.08 and 1.15
    private const int BASE_COST_MULTIPLIER = 10;
    private const int BASE_RATE_MULTIPLIER = 5;
        
    public static Dictionary<int, double> baseCostMap = new Dictionary<int, double>();

    // Used for setup 


    public static double GetBaseCost(int itemNumber) {
        if (itemNumber == 0) {
            return 7;
        } else if (baseCostMap.ContainsKey(itemNumber)) {
            return baseCostMap[itemNumber];
        } else {
            double res = (7 + itemNumber) * GetBaseCost(itemNumber-1); 
            baseCostMap.Add(itemNumber, res);
            return res;
        }
        //return Math.Pow(MULTIPLIER, BASE_RATE_MULTIPLIER * itemNumber);
    }

    const double TIME_TO_PAY_OFF = 5.0;
    // It takes y = 2^x *a seconds to pay of basecost
    // x = itemNumber, y = baseRate, a = amount of seconds
    public static double GetBaseRate(int itemNumber) {
        //return Math.Pow(MULTIPLIER, BASE_RATE_MULTIPLIER * itemNumber);
        //return GetBaseCost(itemNumber) / Mathf.Max( 100 - itemNumber*3,  1) ;
        double baseCost = GetBaseCost(itemNumber);
        //return Math.Log( baseCost/5.0, 2 );

        return baseCost / (TIME_TO_PAY_OFF * Math.Pow(1.2, (itemNumber+1)*2 ));
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
        return Math.Sqrt( goldValue / Math.Pow(10, 10));
    }
}
