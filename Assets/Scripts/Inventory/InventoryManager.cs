using DG.Tweening;
using IS.Inventory.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IS.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public GameObject cellPrefab;
        public GameObject itemPrefab;
        public Transform cellParent;
        public int cellsAmount = 20;
        public ItemDestroy itemDestroy;
        public PanelInfo panelInfo;

        private List<InventoryCell> cells = new List<InventoryCell>();
        
        private void Awake()
        {
            Instance = this;
        }


       private void Start()
        {
            GenerateCells();
            GenerateItems();
        }

       
        /// <summary>
        /// Return random enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <returns></returns>
        public static T GetRandomEnumValue<T>(int min)
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(min, values.Length));
        }
        //Generate cells
        private void GenerateCells()
        {
            for (int i = 0; i < cellsAmount; i++)
            {
                AddCell();
            }

        }
        //Random generate items
        private void GenerateItems()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                int rndItem = UnityEngine.Random.Range(0, ItemDatabase.Instance.itemDatas.Count);
                ItemData itemData = ItemDatabase.Instance.itemDatas[rndItem];

                if(itemData.itemÑategory == ItemÑategory.Animal)
                {
                    AddItem(itemData, 1, GetRandomEnumValue<ItemState>(1));
                }
                else
                {
                    AddItem(itemData, 1);
                }
                
            }
        }
        public void AddCell()
        {
            GameObject newCell = Instantiate(cellPrefab, cellParent);

            InventoryCell cell = newCell.GetComponent<InventoryCell>();
            cells.Add(cell);
        }
        /// <summary>
        /// Adding a new item
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="amount"></param>
        /// <param name="itemState"></param>
        public void AddItem(ItemData itemData, int amount = 1, ItemState itemState = ItemState.None)
        {

            foreach (var cell in cells)
            {
                if (cell.IsEmpty())
                {
                    GameObject newItem = Instantiate(itemPrefab, cell.transform);
                    InventoryItem item = newItem.GetComponent<InventoryItem>();
                    item.Setup(itemData, amount, itemState);
                    item.currentCell = cell;
                    cell.heldItem = item;

                    newItem.transform.localScale = Vector3.zero;
                    newItem.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
                    break;
                }
            }
        }
        /// <summary>
        /// Setting item to an existing cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="itemData"></param>
        /// <param name="amount"></param>
        /// <param name="itemState"></param>
        public void SetItem(InventoryCell cell, ItemData itemData, int amount = 1, ItemState itemState = ItemState.None)
        {
            GameObject newItem = Instantiate(itemPrefab, cell.transform);
            InventoryItem item = newItem.GetComponent<InventoryItem>();
            item.Setup(itemData, amount, itemState);
            item.currentCell = cell;
            cell.heldItem = item;

            item.transform.SetParent(cell.transform);

            newItem.transform.localScale = Vector3.zero;
            newItem.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

        }
        
        //Craft items
        public void TryCraft(InventoryItem a, InventoryItem b)
        {
            if (a.data.type == b.data.type && a.GettState() == b.GettState())
            {
                b.AddAmount(a.amount);
                a.currentCell.heldItem = null;
                Destroy(a.gameObject);
            }
            else
            {
                // Swap items
                InventoryCell cellA = a.currentCell;
                InventoryCell cellB = b.currentCell;

                cellA.heldItem = b;
                cellB.heldItem = a;

                a.currentCell = cellB;
                b.currentCell = cellA;

                a.transform.SetParent(cellB.transform);
                b.transform.SetParent(cellA.transform);

                a.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                b.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
        public void ResetSelectedItems()
        {
            foreach (var cell in cells)
            {
                if (!cell.IsEmpty())
                    cell.heldItem.selected = false;
            }
        }
        /// <summary>
        ///  Return Empty cell
        /// </summary>
        /// <returns></returns>
        private InventoryCell GetEmptyCell()
        {
            foreach (var cell in cells)
            {
                if (cell.IsEmpty()) return cell;
            }
            return null;
        }
        #region UI Events
        /// <summary>
        /// Button add cell click
        /// </summary>
        public void OnBtnAddCellClick()
        {
            AddCell();
        }
        /// <summary>
        /// Button add item click
        /// </summary>
        public void OnBtnAddItemClick()
        {
            InventoryCell cell = GetEmptyCell();
            if (cell)
            {
               
                int rndItem = UnityEngine.Random.Range(0, ItemDatabase.Instance.itemDatas.Count);
                ItemData itemData = ItemDatabase.Instance.itemDatas[rndItem];
                if (itemData.itemÑategory == ItemÑategory.Animal)
                {
                     SetItem(cell, itemData, 1, GetRandomEnumValue<ItemState>(1));
                }
                else
                {
                    SetItem(cell, itemData, 1, ItemState.None);
                }

               
            }
           
        }
        
        #endregion

    }
}


