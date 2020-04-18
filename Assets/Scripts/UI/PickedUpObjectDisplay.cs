using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickedUpObjectDisplay : MonoBehaviour
{
    [SerializeField] PickupObjects playerPickUpObjects;

    [Tooltip("0 = None, 1 = Beer, 2 = Soda, 3 = Chips, 4 = Trash, 5 = Trashbag")]
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
