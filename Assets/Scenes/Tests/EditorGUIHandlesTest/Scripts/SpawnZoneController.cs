using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls spawning behaviour inside the spawning zones.
/// </summary>
[RequireComponent(typeof(EnemySpawnPointController), typeof(BoxCollider))]
public class SpawnZoneController : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<SpawnZoneController> playerEnterEvent;

    private int currentWaveCount;

    public void TriggerSpawn(GameObject[] waveContents)
    {

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
