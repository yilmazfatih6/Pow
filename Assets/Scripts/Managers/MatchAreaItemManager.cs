using Cube;
using Cubes;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "MatchAreaItemManager", menuName = "ScriptableObject/MatchAreaItemManager", order = 0)]
    public class MatchAreaItemManager : ScriptableObject
    {
        [SerializeField] private GameObjectCollection matchAreaItems;
        [SerializeField] private GameObjectCollection matchAreaCubesByAdditionOrder;
        [SerializeField] private GameObjectCollection sceneItems;
        [SerializeField] private GameObjectCollection mergedItems;
        
        [Header("Listened Events")]
        [SerializeField] private GameEvent onRepositionAnimationComplete; 
        [SerializeField] private GameEvent onMergeAnimationComplete;
        [SerializeField] private GameEvent onUndoButtonClick;
        
        [Header("Raised Events")]
        [SerializeField] private GameEvent onItemAdd; 
        [SerializeField] private GameEvent onItemRemove;
        [SerializeField] private GameEvent onMerge;
        [SerializeField] private GameEvent onNotMerge;
        [SerializeField] private GameEvent onUndo;
        
        private int _itemLimit = 7;
        
        private void OnEnable()
        {
            matchAreaItems.Clear();
            matchAreaCubesByAdditionOrder.Clear();
            onRepositionAnimationComplete.AddListener(CheckMerge);
            onMergeAnimationComplete.AddListener(RemoveItems);
            onUndoButtonClick.AddListener(Undo);
        }

        private void OnDisable()
        {
            onRepositionAnimationComplete.RemoveListener(CheckMerge);
            onMergeAnimationComplete.RemoveListener(RemoveItems);
            onUndoButtonClick.RemoveListener(Undo);
        }

        public bool AddItem(GameObject newItem)
        {
            if (matchAreaItems.Count >= _itemLimit) return false;
            
            int targetSlotIndex;
            bool doesSameExist;
            GetTargetIndex(newItem, out doesSameExist, out targetSlotIndex);
                
            matchAreaItems.AddUnique(newItem);
            matchAreaCubesByAdditionOrder.AddUnique(newItem);
            sceneItems.Remove(newItem);
            
            ReorderItems(doesSameExist, newItem, targetSlotIndex);

            onItemAdd.Raise();

            return true;
        }
        
        private void RemoveItems()
        {
            foreach (var item in mergedItems)
            {
                matchAreaItems.Remove(item);
                matchAreaCubesByAdditionOrder.Remove(item);
                Destroy(item);
            }
            
            onItemRemove.Raise();
        }

        private void Undo()
        {
            var count = matchAreaCubesByAdditionOrder.Count;
            
            if(count == 0) return;
            
            onUndo.Raise();

            var lastCube = matchAreaCubesByAdditionOrder[count - 1];
            matchAreaItems.Remove(lastCube);
            matchAreaCubesByAdditionOrder.Remove(lastCube);
            sceneItems.AddUnique(lastCube);
            
            onItemRemove.Raise();
        }

        private void CheckMerge()
        {
            for (int i = 1; i < matchAreaItems.Count - 1; i++)
            {
                // Get textures
                Texture currentItemTexture = matchAreaItems[i].GetComponent<CubeRenderController>().Texture;
                Texture previousItemTexture = matchAreaItems[i-1].GetComponent<CubeRenderController>().Texture;
                Texture nextItemTexture = matchAreaItems[i+1].GetComponent<CubeRenderController>().Texture;
                
                // Merge if prev and next textures are the same.
                if (currentItemTexture == previousItemTexture && currentItemTexture == nextItemTexture)
                {
                    // Clear & Add new merged items
                    mergedItems.Clear();
                    mergedItems.Add(matchAreaItems[i]);
                    mergedItems.Add(matchAreaItems[i-1]);
                    mergedItems.Add(matchAreaItems[i+1]);
                    
                    // Raise event.
                    onMerge.Raise();
                    return;
                }
            }
            
            onNotMerge.Raise();
        }

        private void GetTargetIndex(GameObject newItem, out bool doesSameExist, out int targetSlotIndex)
        {
            targetSlotIndex = -1;
            doesSameExist = false;
            
            // Get target slot index if same type of item exists.
            for (var i = matchAreaItems.Count - 1; i >= 0; i--)
            {
                Texture itemTexture = matchAreaItems[i].GetComponent<CubeRenderController>().Texture;
                Texture newItemTexture = newItem.GetComponent<CubeRenderController>().Texture;

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
                targetSlotIndex = matchAreaItems.Count;
            }
        }

        private void ReorderItems(bool doesSameExist, GameObject newItem, int targetSlotIndex)
        {
            if (!doesSameExist || matchAreaItems.Count <= targetSlotIndex + 1) return;
            
            var temp = matchAreaItems[targetSlotIndex];
            matchAreaItems[targetSlotIndex] = newItem;

            for (int i = matchAreaItems.Count - 1; i > targetSlotIndex + 1; i--)
            {
                matchAreaItems[i] = matchAreaItems[i-1];
            }
            
            matchAreaItems[targetSlotIndex + 1] = temp;
            
            DebugNames();
        }

        private void DebugNames()
        {
            string debugString = "";

            for (int i = 0; i < matchAreaItems.Count; i++)
            {
                debugString += "\nItem " + i + "| Name: " + matchAreaItems[i].GetComponent<CubeRenderController>().Texture.name;
            }
            
            Debug.Log(debugString);
        }
    }
}