using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomInspector;

[CreateAssetMenu(menuName = "THING/Cut/Pattern/Slide")]
public class CutPatternSlide : ACutPattern
{
    public enum SlideDirection
    {
        BottomToTop,
        TopToBottom,
        LeftToRight,
        RightToLeft
    }

    [Space] [HideField,SerializeField] bool _s0;
    [HorizontalLine(color:FixedColor.DustyBlue,message:"SLIDE"),HideField,SerializeField] bool _l0;

    [Space]
    [SerializeField] SlideDirection slideDirection = SlideDirection.BottomToTop;

    [Space]
    [SerializeField] float slideinDuration = 0.5f;
    [SerializeField] Ease slideinEase = Ease.OutQuad;
    
    [Space]
    [HorizontalLine]
    [SerializeField] float slideoutDuration = 0.5f;
    [SerializeField] Ease slideoutEase = Ease.InFlash;


    private Vector2 _endPos;
    private Vector2 _startPos;    
    private Sequence _seq;

    private void OnDisable() => killSequence();    

    private void killSequence()
    {
        if (_seq != null && _seq.IsActive())
        {
            _seq.Kill();
            _seq = null;
        }
    }

    

    private Vector2 GetStartPosition(Vector2 _endPos)
    {
        switch (slideDirection)
        {
            case SlideDirection.BottomToTop:
                return new Vector2(_endPos.x, _endPos.y - Screen.height);
            case SlideDirection.TopToBottom:
                return new Vector2(_endPos.x, _endPos.y + Screen.height);
            case SlideDirection.LeftToRight:
                return new Vector2(_endPos.x - Screen.width, _endPos.y);
            case SlideDirection.RightToLeft:
                return new Vector2(_endPos.x + Screen.width, _endPos.y);
            default:
                return _endPos;
        }
    }

    protected override void OnSet()
    {
        _endPos = cuttransform.localPosition;
        _startPos = GetStartPosition(_endPos);
    }

    public override void Open()
    {        
        killSequence();

        _seq = DOTween.Sequence();

        if (cuttransform != null)
        {
            cuttransform.localPosition = _startPos;
            _seq.Append(cuttransform.DOLocalMove(_endPos, slideinDuration).SetEase(slideinEase));
        }

        _seq.OnComplete(() => {cuttransform.localPosition = _endPos;});
    }

    public override void Close()
    {
        killSequence();

        _seq = DOTween.Sequence();

        if (cuttransform != null)
        {
            cuttransform.localPosition = _endPos;
            _seq.Append(cuttransform.DOLocalMove(_startPos, slideoutDuration).SetEase(slideoutEase));
        }

        _seq.OnComplete(() => 
        {
            cuttransform.localPosition = _startPos;
            cuttransform.parent.gameObject.SetActive(false);
        });
    }
}