using UnityEngine;

[CreateAssetMenu(fileName = "Shop Item", menuName = "Dungeon Escape/Items/Shop Item")]
public class ShopItem : ScriptableObject
{
    public Item gameItem;
    public int cost = 200;
}
