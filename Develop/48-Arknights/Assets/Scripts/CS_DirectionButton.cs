using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_DirectionButton : MonoBehaviour {

    public void OnMouseDown () {
        CS_GameManager.Instance.BeginDragDirection ();
    }

    public void OnMouseUp () {
        CS_GameManager.Instance.EndDragDirection ();
    }

    public void OnMouseDrag () {
        CS_GameManager.Instance.DragDirection ();
    }
}
