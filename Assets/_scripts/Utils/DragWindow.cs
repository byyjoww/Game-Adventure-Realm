using Elysium.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField, ReadOnly] private RectTransform rect;
    [SerializeField, ReadOnly] private Canvas canvas;

    private void OnValidate()
    {
        FindRectTransform();
        FindCanvas();
    }

    private void FindRectTransform()
    {
        if (rect == null)
        {
            rect = transform.parent.GetComponent<RectTransform>();
        }
    }

    private void FindCanvas()
    {
        if (canvas == null)
        {
            Transform testCanvasTransform = transform.parent;
            while (testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if (canvas != null) { break; }
                testCanvasTransform = testCanvasTransform.parent;
            }
        }
        canvas = transform.parent.root.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform is RectTransform == false) { throw new System.Exception($"No rectTransform attached to object {gameObject}. DragWindow script requires a rectTransform."); }
        if (canvas == null) { FindCanvas(); };
        rect.anchoredPosition += (eventData.delta / canvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rect.SetAsLastSibling();
    }
}
