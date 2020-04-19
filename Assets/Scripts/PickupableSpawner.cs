using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableSpawner : MonoBehaviour
{
    [SerializeField] PickupableObjectType typeToSpawn;
    [SerializeField] Transform spawnPosition;

    [Tooltip(GlobalVariables.PickupableObjectTypeTooltip)]
    [SerializeField] List<GameObject> pickupablePrefab;

    GameObject lastSpawned = null;

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

    private void Start()
    {
        SpawnPickupableObject();
    }
}
