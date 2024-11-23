using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


public class DragControl : MonoBehaviour
{

    [Space(30)] [HideField,SerializeField] bool _s0;


    private InteractionDragDrop dragdropcontrol;

    private void OnEnable() 
    {
        dragdropcontrol = GetComponentInParent<InteractionDragDrop>();
        if (dragdropcontrol == null)
            Debug.LogError("DropControl ERROR ] DragDropControl 없음");

        dragdropcontrol.eventDragNDrop += oneventDragNDrop;        
    }

    private void OnDisable() 
    {
        dragdropcontrol.eventDragNDrop += oneventDragNDrop;        
    }


    private void oneventDragNDrop(GameObject drag, GameObject drop)
    {        
        if (drag == null || drop == null || drag.name != gameObject.name) return;

        gameObject.SetActive(false);
    }
}
