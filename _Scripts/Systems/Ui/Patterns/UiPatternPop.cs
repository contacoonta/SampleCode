
using UnityEngine;
using DG.Tweening;
using CustomInspector;


[CreateAssetMenu(menuName = "THING/Ui/Pattern/Pop")]
public class UiPatternPop : AUiPattern
{


    [Space(30)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("UI DIMMER")][HideField,SerializeField] bool _l0;

    [SerializeField][Range(0f,1f)] float dimAlpha = 0.6f;
    [Space]
    [SerializeField] float diminDuration = 0.25f;
    [SerializeField] Ease diminEase = Ease.OutSine;
    [Space]
    [SerializeField] float dimoutDuration = 0.5f;
    [SerializeField] Ease dimoutEase = Ease.InFlash;


    [Space(30)] [HideField,SerializeField] bool _s1;
    [HorizontalLine("UI CANVAS")][HideField,SerializeField] bool _l1;
    [SerializeField] float fadeinDuration = 0.25f;
    [SerializeField] Ease fadeinEase = Ease.OutQuad;
    
    [HorizontalLine]
    [SerializeField] float fadeoutDuration = 0.25f;
    [SerializeField] Ease fadeoutEase = Ease.InFlash;



    [Space(30)] [HideField,SerializeField] bool _s2;
    [HorizontalLine("UI PANEL")][HideField,SerializeField] bool _l2;
    [SerializeField] float scaleinDuration = 0.25f;
    [SerializeField] Ease scaleinEase = Ease.OutBack;

    [HorizontalLine]
    [SerializeField] float scaleoutDuration = 0.25f;
    [SerializeField] Ease scaleoutEase = Ease.InBack;



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
    
    public override void Open()
    {
        uipanel.localScale = Vector3.zero;

        killSequence();

        _seq = DOTween.Sequence();
        _seq.AppendCallback(()=>
        {
            uidimmer?.gameObject.SetActive(true);
            uidimmer?.Set(dimAlpha, diminDuration, diminEase, dimoutDuration, dimoutEase);
            uipanel.gameObject.SetActive(true);
            uipanel.localScale = Vector3.one*0.5f;
            if (uicanvas != null)
                uicanvas.alpha = 0f;
        });        
        _seq.InsertCallback(0f, ()=>uicanvas?.DOFade(1f,fadeinDuration).SetEase(fadeinEase));
        _seq.AppendCallback(()=>uidimmer?.FadeIn());        
        _seq.Append(uipanel.DOScale(1f,scaleinDuration).SetEase(scaleinEase));
    }

    public override void Close()
    {
        killSequence();

        _seq = DOTween.Sequence();
        _seq.AppendCallback(()=>uipanel.localScale = Vector3.one);
        _seq.Append(uipanel.DOScale(0.5f,scaleoutDuration).SetEase(scaleoutEase));
        _seq.InsertCallback(0f, ()=>uicanvas?.DOFade(0f,fadeoutDuration).SetEase(fadeoutEase));
        _seq.AppendCallback(()=>uidimmer?.FadeOut(()=>
        {            
            if (uicanvas != null)
                uicanvas.alpha = 0f;
            uipanel.gameObject.SetActive(false);
            uidimmer?.gameObject.SetActive(false);
        }));
    }

}
