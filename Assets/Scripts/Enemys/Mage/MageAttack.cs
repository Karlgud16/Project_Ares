//Handles the method to spawn in a projectile on the mage

using UnityEngine;

public class MageAttack : MonoBehaviour
{
    private Vector3 _spawnProjectilePosition;

    void Start()
    {
        _spawnProjectilePosition = transform.GetChild(4).position;
    }

    public void SpawnProjectile()
    {
        Instantiate(GameManager.Instance.Projectile, _spawnProjectilePosition, Quaternion.identity);
    }
}
