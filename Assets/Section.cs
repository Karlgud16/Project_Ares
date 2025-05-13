using UnityEngine;

public class Section : MonoBehaviour
{
    [ReadOnly] public int EnemyCount;

    private bool _itemSpawned;

    private Vector3 _spawnerPos;

    private void Awake()
    {
        EnemyCount = transform.GetChild(2).childCount;
    }

    private void Start()
    {
        _itemSpawned = false;
        _spawnerPos = transform.GetChild(0).position;
    }

    void Update()
    {
        EnemyCount = transform.GetChild(2).childCount;

        if (EnemyCount <= 0 && _itemSpawned == false)
        {
            Instantiate(GameManager.Instance.ItemSpawner, _spawnerPos, Quaternion.identity);
            _itemSpawned = true;
        }
    }
}
