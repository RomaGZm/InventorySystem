using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IS.Inventory.SO;
using System;
using System.Collections.Generic;

namespace IS.Inventory
{
    public class PanelInfo : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text texDescription;
        [SerializeField] private TMP_Dropdown dropdownChangeState;
        private InventoryItem currentItem;
        /// <summary>
        /// Settings info data
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="amount"></param>
        /// <param name="itemState"></param>
        public void SetData(InventoryItem item)
        {
            image.sprite = item.data.icon;
            texDescription.text = "Name - " + item.data.itemName + "\n"
                 + "ID - " + item.data.itemId + "\n"
                 + "Stack - " + item.data.itemStack + "\n"
                 + "Amount - " + item.amount + "\n"
                 + "Type - " + item.data.type + "\n"
                 + "Category - " + item.data.item—ategory;
                
          
            dropdownChangeState.interactable = item.data.item—ategory == Item—ategory.Animal;
            string[] names = Enum.GetNames(typeof(ItemState));

            dropdownChangeState.options.Clear();
            dropdownChangeState.AddOptions(new List<string>(names));
            dropdownChangeState.RefreshShownValue();
            SetValueFromEnum(item.itemState);
            currentItem = item;

        }
        private void SetValueFromEnum<T>(T enumValue) where T : Enum
        {
            string enumName = enumValue.ToString();
            int index = dropdownChangeState.options.FindIndex(option => option.text == enumName);

            if (index >= 0)
            {
                dropdownChangeState.SetValueWithoutNotify(index);
                dropdownChangeState.RefreshShownValue();
            }
        }
        private T GetSelectedEnum<T>() where T : Enum
        {
            string selectedText = dropdownChangeState.options[dropdownChangeState.value].text;
            return (T)Enum.Parse(typeof(T), selectedText);
        }
        public void OnValueChange(int value_)
        {
            if (currentItem)
            {
                currentItem.SetState(GetSelectedEnum<ItemState>()); 
            }
            print(value_ + "  " + currentItem.name + "  " + GetSelectedEnum<ItemState>());
        
        }
    }
}


