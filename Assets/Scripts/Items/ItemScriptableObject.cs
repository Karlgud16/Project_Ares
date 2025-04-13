//Handles the values of items
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string ItemName;
    public Sprite ItemSprite;
    public int ItemCost;
}
