//Spawns in the Item Spawner + moves the camera again when all enemys are dead in a section

using UnityEngine;

public class Section : MonoBehaviour
{
    [ReadOnly] public int EnemyCount;

    private bool _itemSpawned;

    private Vector3 _spawnerPos;

    private ItemManager _itemManager;

    private GameObject _rightCollider;

    private CameraManager _cameraManager;

    private void Awake()
    {
        EnemyCount = transform.GetChild(2).childCount;

        _rightCollider = transform.GetChild(1).transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();

        _itemSpawned = false;
        _spawnerPos = transform.GetChild(0).position;

        _cameraManager = GameManager.Instance.GetComponent<CameraManager>();
    }

    void Update()
    {
        EnemyCount = transform.GetChild(2).childCount;

        if (EnemyCount <= 0 && _itemSpawned == false)
        {
            Instantiate(_itemManager.ItemSpawner, _spawnerPos, Quaternion.identity);
            _rightCollider.SetActive(false);
            _itemSpawned = true;

            if(_cameraManager.CameraMove == false)
            {
                _cameraManager.CameraMove = true;
            }
        }
    }
}
