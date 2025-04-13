//Starts the Summoners summoning >:)

using UnityEngine;

public class SummonerStartSummon : MonoBehaviour
{
    [SerializeField] private SummonEnemies _summonEnemies;

    void Awake()
    {
        _summonEnemies = transform.root.GetComponent<SummonEnemies>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _summonEnemies.StartSummon = true;
        }
    }
}
