using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class WorldButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onPressed;

    public void OnPointerDown(PointerEventData eventData) => onPressed.Invoke();
}
