using System;
using Cube;
using Cubes;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "MatchAreaAnimationManager", menuName = "ScriptableObject/MatchAreaAnimationManager", order = 0)]
    public class MatchAreaAnimationManager : ScriptableObject
    {
        [SerializeField] private MatchAreaManager matchAreaManager;
        [SerializeField] private GameObjectCollection items;
        [SerializeField] private float movementDuration = 0.5f;
        [SerializeField] private GameObjectCollection mergedItems;
        [SerializeField] private GameObjectCollection matchAreaCubesByAdditionOrder;

        [Header("Raised Events")]
        [SerializeField] private GameEvent onRepositionAnimationComplete; 
        [SerializeField] private GameEvent onMergeAnimationComplete;
        
        [Header("Listened Events")]
        [SerializeField] private GameEvent onItemAdd; 
        [SerializeField] private GameEvent onItemRemove;
        [SerializeField] private GameEvent onMerge;
        [SerializeField] private GameEvent onUndo;

        private void OnEnable()
        {
            // onItemAdd.AddListener(RepositionItems);
            // onItemRemove.AddListener(RepositionItems);
            onMerge.AddListener(MergeItems);
            onUndo.AddListener(ReplaceUndoItem);
        }
        
        private void OnDestroy()
        {
            // onItemAdd.RemoveListener(RepositionItems);
            // onItemRemove.RemoveListener(RepositionItems);
            onMerge.RemoveListener(MergeItems);
            onUndo.RemoveListener(ReplaceUndoItem);
        }

        // private Tween MoveItem(GameObject item, Transform targetTransform)
        // {
        //     
        // }

        

        private void MergeItems()
        {
            // MoveItem(mergedItems[1], mergedItems[0].transform);
            // MoveItem(mergedItems[2], mergedItems[0].transform).OnComplete(() => onMergeAnimationComplete.Raise());
        }

        private void ReplaceUndoItem()
        {
            var count = matchAreaCubesByAdditionOrder.Count;
            var cube = matchAreaCubesByAdditionOrder[count - 1];
            var cubeMovement = cube.GetComponent<CubeMovementController>();
            cubeMovement.MoveToOriginalPosition();
        }
    }
}