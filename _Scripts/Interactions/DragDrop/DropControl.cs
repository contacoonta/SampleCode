using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using CustomInspector;
using DG.Tweening;


public class DropControl : MonoBehaviour
{

    [Space(20)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("드랍 후 활성")][HideField,SerializeField] bool _l0;
    [SerializeField] List<GameObject> activate;


    [Space(5)] [HideField,SerializeField] bool _s1;
    [HorizontalLine("드랍 후 비활성")][HideField,SerializeField] bool _l1;
    [SerializeField] List<GameObject> deactivate;

    [Space(30)] [HideField,SerializeField] bool _s2;

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




    public void OnDropped(GameObject dragobj, LeanFinger finger)
    {
        if (dragobj.name != gameObject.name) return;

        dragdropcontrol.TriggerDropped(dragobj, gameObject);
    }

    private void oneventDragNDrop(GameObject drag, GameObject drop)
    {
        if (drag.name != gameObject.name) return;

        foreach(var deact in deactivate)
            deact?.SetActive(false);

        foreach(var act in activate)
        {
            act?.SetActive(true);
            act.GetComponent<SpriteRenderer>().sortingOrder += 10;
            act.transform.DOPunchScale(Vector3.up, 0.3f).OnComplete(()=>act.transform.DOScale(1f,0.15f));
        }
    }
}
