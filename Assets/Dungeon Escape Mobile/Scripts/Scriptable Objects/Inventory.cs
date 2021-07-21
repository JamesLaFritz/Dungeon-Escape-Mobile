using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Dungeon Escape/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<Item> m_items = new List<Item>();

    public void Add(Item item)
    {
        m_items.Add(item);
    }

    public void Remove(Item item)
    {
        m_items.Remove(item);
    }

    public void EmptyInventory()
    {
        m_items = new List<Item>();
    }
}
