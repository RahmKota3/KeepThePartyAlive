using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSeed : MonoBehaviour
{
    public bool Randomize = true;

    void GenerateRandomSeed()
    {
        if (Randomize == false)
            return;

        Random.InitState(Random.Range(int.MinValue, int.MaxValue));
    }

    private void Awake()
    {
        LevelManager.Instance.OnAfterSceneLoad += GenerateRandomSeed;
    }
}
