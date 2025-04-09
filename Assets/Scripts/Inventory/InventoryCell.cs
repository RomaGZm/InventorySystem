using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IS.Inventory
{
    [System.Serializable]
    public class InventoryCell : MonoBehaviour
    {
        public InventoryItem heldItem;

        // Check if the cell is empty
        public bool IsEmpty()
        {
            return heldItem == null;
        }
    }
}

