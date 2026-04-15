using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class WorldButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onPressed;

    private void Start()
    {
        onPressed.AddListener(() => print("a"));
    }
    public void OnPointerDown(PointerEventData eventData) => onPressed.Invoke();
}
