using System;
using Cubes;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "ItemManager", menuName = "ScriptableObject/ItemManager", order = 0)]
    public class ItemManager : ScriptableObject
    {
        [SerializeField] private GameObjectCollection items;
        [SerializeField] private GameEvent onItemsChange;
        [SerializeField] private GameObjectGameEvent onCubeSelect;
        private int _itemLimit = 7;
        
        private void OnEnable()
        {
            onCubeSelect.AddListener(AddItem);
            items.Clear();
        }

        private void OnDisable()
        {
            onCubeSelect.RemoveListener(AddItem);
        }

        public void AddItem(GameObject newItem)
        {
            if (items.Count >= _itemLimit) return;
            
            int targetSlotIndex;
            bool doesSameExist;
            GetTargetIndex(newItem, out doesSameExist, out targetSlotIndex);
                
            items.AddUnique(newItem);
            
            ReorderItems(doesSameExist, newItem, targetSlotIndex);
            
            DebugNames();

            onItemsChange.Raise();
        }

        private void GetTargetIndex(GameObject newItem, out bool doesSameExist, out int targetSlotIndex)
        {
            targetSlotIndex = -1;
            doesSameExist = false;
            
            // Get target slot index if same type of item exists.
            for (var i = items.Count - 1; i >= 0; i--)
            {
                Texture itemTexture = items[i].GetComponent<CubeData>().Texture;
                Texture newItemTexture = newItem.GetComponent<CubeData>().Texture;

                if (itemTexture == newItemTexture)
                {
                    doesSameExist = true;
                    targetSlotIndex = i + 1;
                    break;
                }
            }

            // Else set next empty slot as index.
            if (targetSlotIndex == -1)
            {
                targetSlotIndex = items.Count;
            }
        }

        private void ReorderItems(bool doesSameExist, GameObject newItem, int targetSlotIndex)
        {
            if (!doesSameExist || items.Count <= targetSlotIndex + 1) return;
            
            var temp = items[targetSlotIndex];
            items[targetSlotIndex] = newItem;

            for (int i = items.Count - 1; i > targetSlotIndex + 1; i--)
            {
                items[i] = items[i-1];
            }
            
            items[targetSlotIndex + 1] = temp;
        }

        private void DebugNames()
        {
            string debugString = "";

            for (int i = 0; i < items.Count; i++)
            {
                debugString += "\nItem " + i + "| Name: " + items[i].GetComponent<CubeData>().Texture.name;
            }
            
            Debug.Log(debugString);
        }
    }
}