using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class WorldButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onPressed;
    public bool canBePressed = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canBePressed) onPressed.Invoke();
    }
}
