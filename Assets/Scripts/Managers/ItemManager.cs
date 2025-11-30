//Handles all of the changes an item will make

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [Header("Items")]
    public List<ItemList> Items = new List<ItemList>();
    public GameObject ItemUIPrefab;
    public GameObject HealthPickupObject;
    [ReadOnly] public Transform ItemUIBackground;
    public List<GameObject> ItemDrops;

    [Header("ItemsStats")]
    public GameObject ItemSpawner;
    public float HealthPickup;
    public float HealthPickupMultiplier;
    public float AnubisAnkhHealth;
    [Range(0.01f, 0.99f)] public float SlimeArmourMultiplier;
    [Range(0, 1)][SerializeField] private float _SlimeArmourSameItemMultiplier;
    public float SlimeArmourSecondsUntilNormalSpeed;

    private PlayerManager _playerManager;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        ItemUIBackground = GameObject.FindGameObjectWithTag("HUD").transform.Find("OwnedItems").transform.GetChild(0);

        if (Items.Count < 1)
        {
            return;
        }
        else
        {
            InstantiateItemUI();
        }
    }

    void Start()
    {
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
        StartCoroutine(CallItemUpdateOneSecond());
        StartCoroutine(CallItemUpdateOneTick());
    }

    IEnumerator CallItemUpdateOneSecond()
    {
        foreach (ItemList i in Items)
        {
            i.item.UpdateOneSecond(this, i.stacks);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(CallItemUpdateOneSecond());
    }

    IEnumerator CallItemUpdateOneTick()
    {
        foreach (ItemList i in Items)
        {
            i.item.UpdateOneTick(this, i.stacks);
        }
        yield return new WaitForSeconds(0.1f);
    }

    private void InstantiateItemUI()
    {
        foreach (ItemList i in Items)
        {
            GameObject _itemUI = Instantiate(ItemUIPrefab, ItemUIBackground);
            _itemUI.GetComponent<Image>().sprite = i.item.ItemSprite();
            _itemUI.GetComponent<Image>().color = Color.white;
            _itemUI.name = i.item.GetName();

            if (_itemUI.transform.childCount > 0)
            {
                foreach (Transform child in _itemUI.transform)
                {
                    if (child.gameObject.name.Contains("Multiplier"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }
}
