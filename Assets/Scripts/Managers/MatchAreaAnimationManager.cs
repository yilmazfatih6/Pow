using System;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "MatchAreaAnimationManager", menuName = "ScriptableObject/MatchAreaAnimationManager", order = 0)]
    public class MatchAreaAnimationManager : ScriptableObject
    {
        [SerializeField] private MatchAreaItemManager matchAreaItemManager;
        [SerializeField] private GameObjectCollection slots;
        [SerializeField] private GameObjectCollection items;
        [SerializeField] private float movementDuration = 0.5f;
        [SerializeField] private GameObjectCollection mergedItems;

        [Header("Raised Events")]
        [SerializeField] private GameEvent onRepositionAnimationComplete; 
        [SerializeField] private GameEvent onMergeAnimationComplete;
        
        [Header("Listened Events")]
        [SerializeField] private GameEvent onItemAdd; 
        [SerializeField] private GameEvent onItemRemove;
        [SerializeField] private GameEvent onMerge;

        private void OnEnable()
        {
            onItemAdd.AddListener(RepositionItems);
            onItemRemove.AddListener(RepositionItems);
            onMerge.AddListener(MergeItems);
        }
        
        private void OnDestroy()
        {
            onItemAdd.RemoveListener(RepositionItems);
            onItemAdd.RemoveListener(RepositionItems);
            onMerge.RemoveListener(MergeItems);
        }

        private Tween MoveItem(GameObject item, Transform targetTransform)
        {
            item.transform.SetParent(targetTransform);
            item.transform.localScale = Vector3.one;
            item.transform.DOMove(targetTransform.position, movementDuration);
            return item.transform.DORotateQuaternion(targetTransform.rotation, movementDuration);
        }

        private void RepositionItems()
        {
            Tween tween = null;

            for (int i = 0; i < items.Count; i++)
            {
                if(i == 0) tween = MoveItem(items[i], slots[i].transform);
                else MoveItem(items[i], slots[i].transform);
            }
            
            tween.OnComplete(() => onRepositionAnimationComplete.Raise());
        }

        private void MergeItems()
        {
            MoveItem(mergedItems[1], mergedItems[0].transform);
            MoveItem(mergedItems[2], mergedItems[0].transform).OnComplete(() => onMergeAnimationComplete.Raise());
        }
    }
}