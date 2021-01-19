using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_PlayerButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [SerializeField] int myIndex;
    

    public void OnBeginDrag (PointerEventData eventData) {
        CS_GameManager.Instance.BeginDragPlayer ();
    }

    public void OnDrag (PointerEventData eventData) {
        CS_GameManager.Instance.DragPlayer ();
    }

    public void OnEndDrag (PointerEventData eventData) {
        CS_GameManager.Instance.EndDragPlayer ();
    }

    public void OnPointerDown (PointerEventData eventData) {
        CS_GameManager.Instance.SetMyCurrentPlayer (myIndex);
    }

}
