using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTowardsQuest : MonoBehaviour
{
    public Vector3 questWorldPosition = new Vector3(-100, 0, -24);

    Camera cam;

    RectTransform rectTransform;

    private void Awake()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(transform.position);
        Vector3 questScreenPosition = cam.WorldToScreenPoint(questWorldPosition);
        bool isOffScreen = questScreenPosition.x <= 0 || questScreenPosition.x >= Screen.width || questScreenPosition.y <= 0 ||
            questScreenPosition.y >= Screen.height;
        Debug.Log(isOffScreen);

        if (isOffScreen)
        {
            Vector3 cappedScreenPos = questScreenPosition;
            if (cappedScreenPos.x <= 0)
                cappedScreenPos.x = 0;
            if (cappedScreenPos.x >= Screen.width)
                cappedScreenPos.x = Screen.width;
            if (cappedScreenPos.y <= 0)
                cappedScreenPos.y = 0;
            if (cappedScreenPos.y >= Screen.height)
                cappedScreenPos.y = Screen.height;

            transform.position = cappedScreenPos;
        }
        else
        {
            transform.position = questScreenPosition;
        }
    }
}
