using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject glow;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private float movementDuration = 0.5f;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        glow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glow.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        glow.SetActive(false);

        var targetTransform = slotManager.AddItem(gameObject);
        
        transform.SetParent(targetTransform);
        transform.localScale = Vector3.one;
        transform.DOMove(targetTransform.position, movementDuration);
        transform.DORotateQuaternion(targetTransform.rotation, movementDuration);

        SetLayer();
        
        enabled = false;
    }

    private void SetLayer()
    {
        foreach (var t in GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = 5;
        }
    }
}