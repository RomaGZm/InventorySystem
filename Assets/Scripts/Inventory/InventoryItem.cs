using DG.Tweening;
using IS.Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace IS.Inventory
{


    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public InventoryCell currentCell;
        [Header("Data")]
        public ItemData data;
        public int amount = 1;
        public ItemState itemState;
        [Header("Components")]
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image selectImage;
        [SerializeField] private Image stateImage;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;

        private Transform originalParent;

        private bool selected_ = false;
        public bool selected
        {
            get { return selected_; }
            set 
            {
                selected_ = value;
                selectImage.gameObject.SetActive(selected_);
            }
        }
        /// <summary>
        /// Set current item State
        /// </summary>
        /// <param name="state"></param>
        public void SetState(ItemState state)
        {
            itemState = state;
            stateImage.gameObject.SetActive(state == ItemState.Wounded);
        }
        /// <summary>
        /// Return current item State 
        /// </summary>
        /// <returns></returns>
        public ItemState GettState()
        {
            return itemState;
        }
        /// <summary>
        /// Setting the basic values
        /// </summary>
        /// <param name="newData"></param>
        /// <param name="newAmount"></param>
        /// <param name="itemState"></param>
        public void Setup(ItemData newData, int newAmount, ItemState itemState)
        {
            data = newData;
            amount = newAmount;
            itemImage.sprite = data.icon;
            SetState(itemState);
            UpdateAmountText();
        }
        /// <summary>
        /// Add to counter items
        /// </summary>
        /// <param name="value"></param>
        public void AddAmount(int value)
        {
            amount += value;
            UpdateAmountText();
            transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }
        /// <summary>
        /// Update item quantity text
        /// </summary>
        private void UpdateAmountText()
        {
            if (amountText != null)
                amountText.text = amount.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            originalParent = transform.parent;
            transform.SetParent(transform.root);
            InventoryManager.Instance.itemDestroy.StartFade();
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            GameObject hitObj = eventData.pointerCurrentRaycast.gameObject;
            InventoryCell targetCell = null;

            if (hitObj != null)
            {
                ItemDestroy itemDestroy = hitObj.GetComponent<ItemDestroy>();

                if (itemDestroy)
                {
                    Destroy(gameObject);
                    InventoryManager.Instance.itemDestroy.StopFade();
                    return;
                }

                targetCell = hitObj.GetComponentInParent<InventoryCell>();
            }
            
            if (targetCell != null && currentCell != targetCell)
            {
               
                if (targetCell.IsEmpty())
                {
                    currentCell.heldItem = null;
                    currentCell = targetCell;
                    targetCell.heldItem = this;
                    transform.SetParent(targetCell.transform);
                    rectTransform.anchoredPosition = Vector2.zero;
                }
                else
                {
                    InventoryItem targetItem = targetCell.heldItem;
                    InventoryManager.Instance.TryCraft(this, targetItem);
                }
            }
            else
            {
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = Vector2.zero;
            }
            InventoryManager.Instance.itemDestroy.StopFade();
        }
        
        //Selected item
        public void OnPointerDown(PointerEventData eventData)
        {
            InventoryManager.Instance.ResetSelectedItems();
            InventoryManager.Instance.panelInfo.SetData(this);
            selected = true;
        }
    }
}

