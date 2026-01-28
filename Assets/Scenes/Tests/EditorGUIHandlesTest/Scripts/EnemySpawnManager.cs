using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Oversees spawning across the level using the director.
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance { get; private set; }

    [SerializeField] private SpawnZoneController activeZoneController;

    private EnemySpawnDirector director;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        director = new EnemySpawnDirector();
    }

    public void RegisterZoneEntry(SpawnZoneController zoneController)
    {
        activeZoneController = zoneController;
        Debug.Log("Zone entry", zoneController.gameObject);

        zoneController.TriggerSpawn(null);
    }
}
