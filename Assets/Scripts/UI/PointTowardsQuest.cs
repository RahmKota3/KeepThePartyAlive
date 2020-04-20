using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTowardsQuest : MonoBehaviour
{
    [SerializeField] Image taskImage;

    public Transform questWorldTransform;
    Vector3 questWorldPosition { get { return questWorldTransform.position + new Vector3(0, 3f, 0); } }

    Camera cam;

    Transform playerTransform;

    [Tooltip(GlobalVariables.QuestTypeTooltip)]
    [SerializeField] List<Sprite> taskSprites = new List<Sprite>(5);

    [Tooltip(GlobalVariables.PickupableObjectTypeTooltip)]
    [SerializeField] List<Sprite> bringItemSprites = new List<Sprite>(8);

    void FollowQuestPosition()
    {
        if (questWorldTransform == null)
            return;

        Vector3 worldPosition = cam.ScreenToWorldPoint(transform.position);
        Vector3 questScreenPosition = cam.WorldToScreenPoint(questWorldPosition);

        //Vector3 offset = new Vector3(0, 150, 0);
        Vector3 offset = Vector3.zero;

        bool isOffScreen = questScreenPosition.x + offset.x <= 0 || questScreenPosition.x + offset.x >= Screen.width ||
            questScreenPosition.y + offset.y <= 0 || questScreenPosition.y + offset.y >= Screen.height;

        if (isOffScreen)
        {
            if (Vector3.Distance(playerTransform.position, questWorldPosition) >= 13f)
            {
                Vector3 tempQuestWorldPos = playerTransform.position + (questWorldPosition - playerTransform.position).normalized * 13f;
                questScreenPosition = cam.WorldToScreenPoint(tempQuestWorldPos);
            }

            float xMultiplier = 1;
            float yMultiplier = 1;

            if (questScreenPosition.x <= 0)
            {
                questScreenPosition.x = 0;
                xMultiplier = -1;
            }
            if (questScreenPosition.x >= Screen.width)
                questScreenPosition.x = Screen.width;

            if (questScreenPosition.y <= 0)
            {
                questScreenPosition.y = 0;
                yMultiplier = -1;
            }
            if (questScreenPosition.y >= Screen.height)
                questScreenPosition.y = Screen.height;

            offset = new Vector3(-150 * xMultiplier, -150 * yMultiplier, 0);
        }

        questScreenPosition.z = 0;
        transform.position = questScreenPosition + offset;
    }

    public void SetSprite(QuestType type, PickupableObjectType objType)
    {
        if (type != QuestType.GetSomething)
            taskImage.sprite = taskSprites[(int)type];
        else
            taskImage.sprite = bringItemSprites[(int)objType];
    }

    private void Awake()
    {
        cam = Camera.main;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        FollowQuestPosition();
    }

    private void FixedUpdate()
    {
        FollowQuestPosition();
    }
}
