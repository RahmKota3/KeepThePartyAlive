using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModelActivator : MonoBehaviour
{
    [SerializeField] Transform modelsParent;

    public Animator ActiveAnimator;

    void ActivateRandomModel()
    {
        List<GameObject> models = new List<GameObject>();

        foreach (Transform child in modelsParent)
        {
            models.Add(child.gameObject);
        }

        int rand = Random.Range(0, models.Count);
        models[rand].SetActive(true);
        ActiveAnimator = models[rand].GetComponent<Animator>();
    }

    private void Awake()
    {
        ActivateRandomModel();
    }
}
