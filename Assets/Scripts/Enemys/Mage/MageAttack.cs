//Handles the method to spawn in a projectile on the mage

using UnityEngine;

public class MageAttack : MonoBehaviour
{
    private Vector3 _spawnProjectilePosition;

    private BaseEnemyFlip _baseEnemyFlip;

    private EnemyManager _enemyManager;

    private void Awake()
    {
        _baseEnemyFlip = GetComponent<BaseEnemyFlip>();
    }

    private void Start()
    {
        _enemyManager = GameManager.Instance.GetComponent<EnemyManager>();
    }

    private void Update()
    {
        _spawnProjectilePosition = transform.GetChild(4).position;
        if(_spawnProjectilePosition == transform.GetChild(4).position)
        {
            return;
        }
    }

    public void SpawnProjectile()
    {
        var projectile = Instantiate(_enemyManager.Projectile, _spawnProjectilePosition, Quaternion.identity);
        projectile.GetComponent<MageProjectile>().TargetPlayer = _baseEnemyFlip.FindClosestPlayer();
    }
}
