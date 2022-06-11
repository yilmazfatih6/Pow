using System;
using Cubes;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEditor.Build;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "SlotManager", menuName = "ScriptableObject/SlotManager", order = 0)]
    public class SlotManager : ScriptableObject
    {
        [SerializeField] private GameObjectCollection slots;
        [SerializeField] private GameObjectCollection items;
        private int _itemLimit = 7;
        private void OnEnable()
        {
            items.Clear();
        }

        public Transform AddItem(GameObject newItem)
        {
            // Get target slot index if same type of item exists.
            int targetSlotIndex = -1;
            bool doesSameExist = false;
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
            
            items.AddUnique(newItem);

            if (doesSameExist && items.Count > targetSlotIndex + 1)
            {
                var temp = items[targetSlotIndex];
                items[targetSlotIndex] = newItem;
                
                // Shift 
                int i = targetSlotIndex + 1;
                while (i < items.Count)
                {
                    items[i].transform.DOMove(slots[i+1].transform.position, 0.5f);
                    i++;
                }
                
                items[targetSlotIndex + 1] = temp;
                temp.transform.DOMove(slots[targetSlotIndex + 1].transform.position, 0.5f);

            }
            
            return slots[targetSlotIndex].transform;
        }
    }
}