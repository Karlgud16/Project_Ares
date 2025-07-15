//Handles flipping left or right on the enemy depending on where the nearest player is

using UnityEngine;

public class BaseEnemyFlip : MonoBehaviour
{
    [ReadOnly] public bool canFlip;

    private SpriteRenderer sR;

    private GameObject attackCol, playerTrigger, spawnProjectile;

    private PlayerManager _playerManager;

    /// <summary>
    /// Finds the nearest player from the enemy's position
    /// </summary>
    /// <returns></returns>
    public GameObject FindClosestPlayer()
    {
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;
        GameObject closest = null;
        foreach (GameObject player in _playerManager.Players)
        {
            Vector3 difference = player.transform.position - pos;
            float currantDistance = difference.sqrMagnitude;
            if (currantDistance < distance)
            {
                closest = player;
                distance = currantDistance;
            }
        }
        return closest;
    }

    void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
        if (gameObject.tag == "BaseEnemy" || gameObject.tag == "Brute" || gameObject.tag == "Mage")
        {
            attackCol = transform.GetChild(1).gameObject;
            playerTrigger = transform.GetChild(2).gameObject;
        }
        if(gameObject.tag == "Mage")
        {
            spawnProjectile = transform.GetChild(4).gameObject;
        }
    }

    void Start()
    {
        canFlip = true;

        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (canFlip)
        {
            switch (gameObject.tag)
            {
                case "Mage":
                    if (FindClosestPlayer().transform.position.x < transform.position.x && spawnProjectile.transform.localPosition.x != -0.5f)
                    {
                        Vector3 newPos = spawnProjectile.transform.localPosition;
                        newPos.x = -0.5f;
                        spawnProjectile.transform.localPosition = newPos;
                    }
                    else if (FindClosestPlayer().transform.position.x < transform.position.x && spawnProjectile.transform.localPosition.x != 0.5f)
                    {
                        Vector3 newScale = spawnProjectile.transform.localPosition;
                        newScale.x = 0.5f;
                        spawnProjectile.transform.localPosition = newScale;
                    }
                    break;

                default:
                    if (FindClosestPlayer().transform.position.x < transform.position.x && attackCol.transform.localScale.x != -1)
                    {
                        Vector3 newScale = attackCol.transform.localScale;
                        newScale.x = -1;
                        attackCol.transform.localScale = newScale;
                        Vector3 newScaleTrigger = playerTrigger.transform.localScale;
                        newScaleTrigger.x = -1;
                        playerTrigger.transform.localScale = newScaleTrigger;
                    }
                    else if (FindClosestPlayer().transform.position.x > transform.position.x && attackCol.transform.localScale.x != 1)
                    {
                        Vector3 newScaleCol = attackCol.transform.localScale;
                        newScaleCol.x = 1;
                        attackCol.transform.localScale = newScaleCol;
                        Vector3 newScaleTrigger = playerTrigger.transform.localScale;
                        newScaleTrigger.x = 1;
                        playerTrigger.transform.localScale = newScaleTrigger;
                    }
                    break;
            }

            if (FindClosestPlayer().transform.position.x < transform.position.x)
            {
                sR.flipX = true;
            }
            else
            {
                sR.flipX = false;
            }
        }
    }
}
