using UnityEngine;
using DG.Tweening;
using CustomInspector;


[CreateAssetMenu(menuName = "THING/Ui/Pattern/Slide")]
public class UiPatternSlide : AUiPattern
{
    public enum SlideDirection
    {
        BottomToTop,
        TopToBottom,
        LeftToRight,
        RightToLeft
    }


    [Space(30)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("SLIDE")][HideField,SerializeField] bool _l0;
    [SerializeField] float delay = 0f;
    [SerializeField] SlideDirection slideDirection = SlideDirection.BottomToTop;


    [Space(30)] [HideField,SerializeField] bool _s1;
    [HorizontalLine("UI DIMMER")][HideField,SerializeField] bool _l1;

    [SerializeField][Range(0f,1f)] float dimAlpha = 0.6f;
    [Space]
    [SerializeField] float diminDuration = 0.25f;
    [SerializeField] Ease diminEase = Ease.OutSine;
    [Space]
    [SerializeField] float dimoutDuration = 0.5f;
    [SerializeField] Ease dimoutEase = Ease.InFlash;


    [Space(30)] [HideField,SerializeField] bool _s2;
    [HorizontalLine("UI CANVAS")][HideField,SerializeField] bool _l2;
    [SerializeField] float fadeinDuration = 0.25f;
    [SerializeField] Ease fadeinEase = Ease.OutQuad;
    
    [Space]
    [SerializeField] float fadeoutDuration = 0.25f;
    [SerializeField] Ease fadeoutEase = Ease.InFlash;

    [Space(30)] [HideField,SerializeField] bool _s3;
    [HorizontalLine("UI PANEL")][HideField,SerializeField] bool _l3;
    [SerializeField] float slideinDuration = 0.5f;
    [SerializeField] Ease slideinEase = Ease.OutBack;

    [Space]
    [SerializeField] float slideoutDuration = 0.5f;
    [SerializeField] Ease slideoutEase = Ease.InBack;

    

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


    
    private Vector3 GetStartPosition(Vector3 endPosition)
    {
        switch (slideDirection)
        {
            case SlideDirection.BottomToTop:
                return new Vector3(endPosition.x, endPosition.y - Screen.height, endPosition.z);
            case SlideDirection.TopToBottom:
                return new Vector3(endPosition.x, endPosition.y + Screen.height, endPosition.z);
            case SlideDirection.LeftToRight:
                return new Vector3(endPosition.x - Screen.width, endPosition.y, endPosition.z);
            case SlideDirection.RightToLeft:
                return new Vector3(endPosition.x + Screen.width, endPosition.y, endPosition.z);
            default:
                return endPosition;
        }
    }

    public override void Open()
    {
        Vector2 endPosition = uipanel.anchoredPosition;
        Vector2 startPosition = GetStartPosition(endPosition);

        killSequence();

        _seq = DOTween.Sequence();
        _seq.AppendInterval(delay);
        _seq.AppendCallback(()=>
        {
            uidimmer?.gameObject.SetActive(true);
            uidimmer?.Set(dimAlpha, diminDuration, diminEase, dimoutDuration, dimoutEase);
            uipanel.gameObject.SetActive(true);
            uipanel.anchoredPosition = startPosition;
            if (uicanvas != null)
                uicanvas.alpha = 0f;
        });        
        _seq.InsertCallback(delay, ()=>uicanvas?.DOFade(1f,fadeinDuration).SetEase(fadeinEase));
        _seq.AppendCallback(()=>uidimmer?.FadeIn());        
        _seq.Append(uipanel.DOAnchorPos(endPosition, slideinDuration).SetEase(slideinEase));
    }

    public override void Close()
    {
        Vector2 startPosition = uipanel.anchoredPosition;
        Vector2 endPosition = GetStartPosition(startPosition);

        killSequence();

        _seq = DOTween.Sequence();
        _seq.Append(uipanel.DOAnchorPos(endPosition, slideoutDuration).SetEase(slideoutEase));
        _seq.InsertCallback(0f, ()=>uicanvas?.DOFade(0f,fadeoutDuration).SetEase(fadeoutEase));
        _seq.AppendCallback(()=>uidimmer?.FadeOut(()=>
        {          
            if (uicanvas != null)  
                uicanvas.alpha = 0f;
            uipanel.gameObject.SetActive(false);
            uidimmer?.gameObject.SetActive(false);
            uipanel.anchoredPosition = startPosition;
        }));
    }
}