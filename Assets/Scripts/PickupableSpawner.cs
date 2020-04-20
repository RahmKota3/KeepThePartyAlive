using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableSpawner : MonoBehaviour
{
    [SerializeField] PickupableObjectType typeToSpawn;
    [SerializeField] Transform spawnPosition;

    [Tooltip(GlobalVariables.PickupableObjectTypeTooltip)]
    [SerializeField] List<GameObject> pickupablePrefab;

    [SerializeField] Transform objSprite;

    [Tooltip(GlobalVariables.PickupableObjectTypeTooltip)]
    [SerializeField] List<Sprite> pickupableObjectSprites;

    GameObject lastSpawned = null;

    Camera cam;

    private void OnTriggerExit(Collider other)
    {
        if(lastSpawned == null || other.gameObject == lastSpawned)
        {
            SpawnPickupableObject();
        }
    }

    void SpawnPickupableObject()
    {
        PickupableObjects obj = Instantiate(pickupablePrefab[(int)typeToSpawn], spawnPosition.position, 
            Quaternion.identity).GetComponent<PickupableObjects>();

        lastSpawned = obj.gameObject;
    }

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        // TODO: 
        SpawnPickupableObject();

        objSprite.GetComponent<SpriteRenderer>().sprite = pickupableObjectSprites[(int)typeToSpawn];
    }

    private void Update()
    {
        objSprite.LookAt(cam.transform);
    }
}
