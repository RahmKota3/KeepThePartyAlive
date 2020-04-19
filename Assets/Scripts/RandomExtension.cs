using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExtension
{
    public static int Choose(int a, int b)
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            return a;
        else
            return b;
    }

    public static int ChooseWeighted(int a, int b, int weightA, int weightB)
    {
        int rand = Random.Range(0, weightA + weightB);

        if (rand < weightA)
            return a;
        else
            return b;
    }

    public static int ChooseFromMultiple(List<int> numbersToChooseFrom)
    {
        int rand = Random.Range(0, numbersToChooseFrom.Count);

        return numbersToChooseFrom[rand];
    }

    public static int ChooseFromMultipleWeighted(List<int> numbersToChooseFrom, List<int> numbersWeights)
    {
        if(numbersToChooseFrom.Count != numbersWeights.Count)
        {
            Debug.LogError("Wrong amount of arguments " + numbersToChooseFrom.Count + " : " + numbersWeights.Count);
            return int.MinValue;
        }

        int maxWeight = 0;
        int currentWeight = 0;

        foreach (int i in numbersWeights)
        {
            maxWeight += i;
        }

        int rand = Random.Range(0, maxWeight);

        for (int j = 0; j < numbersWeights.Count; j++)
        {
            currentWeight += numbersWeights[j];

            if (rand <= currentWeight)
                return numbersToChooseFrom[j];
        }

        Debug.LogError("Something went wrong here.");
        return int.MinValue;
    }
    
}
