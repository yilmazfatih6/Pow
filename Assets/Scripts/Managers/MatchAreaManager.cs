using System.Collections.Generic;
using Cube;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "MatchAreaManager", menuName = "ScriptableObject/MatchAreaManager", order = 0)]
    public class MatchAreaManager : ScriptableObject
    {
        [Header("SO Data")] 
        [SerializeField] private GameObjectCollection matchAreaCubes;
        [SerializeField] private GameObjectCollection mergedCubes;
        [SerializeField] private GameObjectCollection slots;

        [Header("Listened Events")]
        [SerializeField] private GameObjectGameEvent onSelect; 
        [SerializeField] private GameEvent onUndoButtonClick;
        
        [Header("Raised Events")]
        [SerializeField] private GameObjectGameEvent onAdd; 
        [SerializeField] private GameObjectGameEvent onRemove;
        [SerializeField] private GameObjectGameEvent onUndo;
        [SerializeField] private GameEvent onMerge;
        [SerializeField] private GameEvent onNotMerge;
        
        private int _cubeLimit = 7;
        private List<GameObject> _cubeInAdditionOrder = new List<GameObject>();

        private void OnEnable()
        {
            // Clear collections
            slots.Clear();
            matchAreaCubes.Clear();
            _cubeInAdditionOrder.Clear();
            
            // Add listener to events
            onSelect.AddListener(Add);
            onUndoButtonClick.AddListener(Undo);
        }

        private void OnDisable()
        {
            // Remove listener to events
            onSelect.RemoveListener(Add);
            onUndoButtonClick.RemoveListener(Undo);
        }

        /// <summary>
        /// Adds cube to match area cubes collections.
        /// </summary>
        private void Add(GameObject newCube)
        {
            if (matchAreaCubes.Count >= _cubeLimit) return;
            
            int targetSlotIndex;
            bool doesSameExist;
            GetTargetIndex(newCube, out doesSameExist, out targetSlotIndex);
                
            matchAreaCubes.Insert(targetSlotIndex, newCube);
            _cubeInAdditionOrder.Add(newCube);
            
            Reorder(doesSameExist, newCube, targetSlotIndex).OnComplete(Merge);

            onAdd.Raise(newCube);
        }

        private void Remove(GameObject cube)
        {
            matchAreaCubes.Remove(cube);
            _cubeInAdditionOrder.Remove(cube);
            onRemove.Raise(cube);
        }
        
        private void Undo()
        {
            var count = _cubeInAdditionOrder.Count;
            if(count == 0) return;
            
            // Move to original position
            var lastCube = _cubeInAdditionOrder[count - 1];
            var cubeMovement = lastCube.GetComponent<CubeMovementController>();
            cubeMovement.MoveToOriginalPosition();
            
            Remove(lastCube);

            onUndo.Raise();
        }

        private void Merge()
        {
            for (int i = 1; i < matchAreaCubes.Count - 1; i++)
            {
                // Get current, prev & next cubes 
                GameObject currentCube = matchAreaCubes[i - 1];
                GameObject previousCube = matchAreaCubes[i];
                GameObject nextCube = matchAreaCubes[i + 1];
                
                // Get current, prev & next cube textures
                Texture currentTexture = currentCube.GetComponent<CubeRenderController>().Texture;
                Texture previousTexture = previousCube.GetComponent<CubeRenderController>().Texture;
                Texture nextTexture = nextCube.GetComponent<CubeRenderController>().Texture;
                
                // Merge if prev and next textures are the same.
                if (currentTexture == previousTexture && currentTexture == nextTexture)
                {
                    mergedCubes.Clear();
                    mergedCubes.Add(previousCube);
                    mergedCubes.Add(currentCube);
                    mergedCubes.Add(nextCube);
                    
                    previousCube.GetComponent<CubeMovementController>().Merge(currentCube.transform);
                    nextCube.GetComponent<CubeMovementController>().Merge(currentCube.transform).OnComplete(() =>
                    {
                        Remove(previousCube);
                        Remove(currentCube);
                        Remove(nextCube);
                        
                        Destroy(previousCube);
                        Destroy(currentCube);
                        Destroy(nextCube);

                        RepositionAll();
                        
                        // Raise event.
                        onMerge.Raise();
                    });
                    
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
            for (var i = matchAreaCubes.Count - 1; i >= 0; i--)
            {
                Texture itemTexture = matchAreaCubes[i].GetComponent<CubeRenderController>().Texture;
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
                targetSlotIndex = matchAreaCubes.Count;
            }
        }

        private Tween Reorder(bool doesSameExist, GameObject newItem, int targetSlotIndex)
        {
            // Move cube.
            var cubeMovement = matchAreaCubes[targetSlotIndex].GetComponent<CubeMovementController>();
            var movementTween = cubeMovement.MoveToMatchArea(slots[targetSlotIndex].transform);

            // Shift cubes.
            int index = targetSlotIndex;
            while (index < matchAreaCubes.Count)
            {
                cubeMovement = matchAreaCubes[index].GetComponent<CubeMovementController>();
                cubeMovement.MoveToMatchArea(slots[index].transform);
                index++;
            }
            
            return movementTween;
        }

        private void RepositionAll()
        {
            for (int i = 0; i < matchAreaCubes.Count; i++)
            {
                var cubeMovement = matchAreaCubes[i].GetComponent<CubeMovementController>();
                cubeMovement.MoveToMatchArea(slots[i].transform);
            }
        }
        
        private void DebugNames()
        {
            string debugString = "";

            for (int i = 0; i < matchAreaCubes.Count; i++)
            {
                debugString += "\nItem " + i + "| Name: " + matchAreaCubes[i].GetComponent<CubeRenderController>().Texture.name;
            }
            
            Debug.Log(debugString);
        }
    }
}