using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls spawning behaviour inside the spawning zones.
/// </summary>
[RequireComponent(typeof(SpawnPointPlacementController), typeof(BoxCollider))]
public class SpawnZoneController : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<SpawnZoneController> playerEnterEvent;

    private int currentWaveCount;

    public void TriggerSpawn(GameObject[] waveContents)
    {
        if (waveContents == null)
        {
            return;
        }

        BoxCollider collider = GetComponent<BoxCollider>();

        float boundsX = collider.bounds.max.x;
        float boundsZ = collider.bounds.max.z;

        foreach (var enemy in waveContents)
        {
            float randomX = Random.Range(-boundsX, boundsX);
            float randomZ = Random.Range(-boundsZ, boundsZ);
            float minY = enemy.GetComponent<BoxCollider>().bounds.extents.y * enemy.transform.localScale.x;

            enemy.transform.position = new Vector3(randomX, minY, randomZ);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        playerEnterEvent?.Invoke(this);
    }

    private void Start()
    {
        EnemySpawnManager manager = EnemySpawnManager.instance;
        if (manager == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("No Spawn Manager in Scene!", this);
#endif
            gameObject.SetActive(false);
            return;
        }

        playerEnterEvent = new UnityEvent<SpawnZoneController>();
        playerEnterEvent.AddListener(manager.RegisterZoneEntry);
    }
}
