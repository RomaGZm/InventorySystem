using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IS.Inventory;

namespace IS.Inventory.SO
{
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemType type;
        public Item—ategory item—ategory;
        public int itemId;
        public int itemStack;
    }

    public enum ItemType
    {
        Gold,
        Wood,
        Ore,
        Beaver,
        Frog,
        Cat,
        Healing,
        Bomb
    }
    public enum Item—ategory
    {
        Resource, Animal, Consumable
    }

    public enum ItemState
    {
        None, Wounded, Healthy
    }
}

