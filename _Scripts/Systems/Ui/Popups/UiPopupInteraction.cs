
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CustomInspector;
using TMPro;


public class UiPopupInteraction : UiPopup
{

    [Space(30)] [HideField,SerializeField] bool _s11;
    [HorizontalLine("RectTransfom 좌표계 관련")][HideField,SerializeField] bool _l11;


    //ScaleFactor : 5-> ≒30, 10-> ≒9, 15-> ≒4
    public AnimationCurve scalefactor;

    private Camera _camera;
    private GameObject _interation;


    private Vector3 _offsetlocal;
    public override void OnOpened(object obj) 
    {
        if (obj is GameObject interaction == false)
            return;

        _camera = Camera.main;
        _interation = interaction;

        _interation.SetActive(true);

        var ainter = _interation.GetComponent<AInteraction>();
        if (ainter == null)
            Debug.LogError("UiPopupInteraction ERROR ] AInteraction 없음");

        _offsetlocal = ainter.Offset;
                
        AdjustScale(_interation.transform);
        AdjustPriority();
    }


    public override void OnClosed() 
    {
        _interation.transform.SetParent(null);
        Destroy(_interation);
    }


    public void AdjustScale(Transform from)
    {
        if (from == null || panel == null || _camera == null)
        {
            Debug.LogError("Target Transform, Parent RectTransform, or Main Camera is not assigned.");
            return;
        }

        float projectionSize = _camera.orthographicSize;

        from.SetParent(panel, false);

        RectTransform targetRectTransform = from as RectTransform;
        if (targetRectTransform == null)
            targetRectTransform = from.gameObject.AddComponent<RectTransform>();        

        targetRectTransform.anchoredPosition = _offsetlocal;
        targetRectTransform.localRotation = Quaternion.identity;
        targetRectTransform.localScale = Vector3.one * projectionSize * scalefactor.Evaluate(projectionSize);
    }

    public void AdjustPriority()
    {
        SpriteRenderer[] spriteRenderers = panel.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            spriteRenderer.sortingOrder += 10;

        TextMeshPro[] tmps = panel.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro tmp in tmps)
            tmp.sortingOrder += 10;
    }

}
