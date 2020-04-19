using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickedUpObjectDisplay : MonoBehaviour
{
    [SerializeField] PickupObjects playerPickUpObjects;

    [Tooltip(GlobalVariables.PickupableObjectTypeTooltip)]
    [SerializeField] List<Sprite> pickupableObjectSprites;

    [SerializeField] Image pickedUpObjectDisplay;

    void ChangeSprite(PickupableObjectType type)
    {
        pickedUpObjectDisplay.sprite = pickupableObjectSprites[(int)type];
    }

    private void Awake()
    {
        playerPickUpObjects.OnDrop += ChangeSprite;
        playerPickUpObjects.OnPickUp += ChangeSprite;
    }
}
