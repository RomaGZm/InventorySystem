using IS.Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IS.Inventory.SO.Data;

namespace IS.Inventory
{
    public class ItemDatabase : MonoBehaviour
    {
        public static ItemDatabase Instance;

        public List<ItemData> itemDatas;

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Returns item by type
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public ItemData GetItemData(ItemType itemType)
        {
            foreach (ItemData item in itemDatas)
            {
                if (item.type == itemType) return item;
            }
            return null;
        }

    }
}
