using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "SlotManager", menuName = "ScriptableObject/SlotManager", order = 0)]
    public class SlotManager : ScriptableObject
    {
        [SerializeField] private GameObjectCollection slots;
        [SerializeField] private GameObjectCollection items;
        [SerializeField] private GameEvent onItemsChange;
        [SerializeField] private float movementDuration = 0.5f;

        private void OnEnable()
        {
            onItemsChange.AddListener(RepositionItems);
        }

        private void OnDisable()
        {
            onItemsChange.RemoveListener(RepositionItems);
        }

        private void RepositionItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                MoveItem(items[i], slots[i].transform);
            }
        }
        
        private void MoveItem(GameObject item, Transform targetTransform)
        {
            item.transform.SetParent(targetTransform);
            item.transform.localScale = Vector3.one;
            item.transform.DOMove(targetTransform.position, movementDuration);
            item.transform.DORotateQuaternion(targetTransform.rotation, movementDuration);
        }
    }
}