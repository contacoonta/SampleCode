
using UnityEngine;
using DG.Tweening;
using CustomInspector;


[CreateAssetMenu(menuName = "THING/Cut/Pattern/Fade")]
public class CutPatternFade : ACutPattern
{

    [Space] [HideField,SerializeField] bool _s0;
    [HorizontalLine(color:FixedColor.DustyBlue,message:"FADE"),HideField,SerializeField] bool _l0;
    [SerializeField] float fadeinDuration = 0.5f;
    [SerializeField] Ease fadeinEase = Ease.OutQuad;
    
    [HorizontalLine]
    [SerializeField] float fadeoutDuration = 0.5f;
    [SerializeField] Ease fadeoutEase = Ease.InFlash;



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

    private void transparentAll(float alpha)
    {
        if (cutsprites != null && cutsprites.Count > 0) 
            foreach( var sprite in cutsprites )
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

        if (cuttexts != null && cuttexts.Count > 0)
            foreach (var text in cuttexts)
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }

    protected override void OnSet()
    {
        transparentAll(0f);
    }

    public override void Open()
    {        
        killSequence();

        _seq = DOTween.Sequence();
        _seq.AppendCallback(() => transparentAll(0f));

        foreach (var sprite in cutsprites)
            _seq.Join(sprite.DOFade(1f, fadeinDuration).SetEase(fadeinEase));

        foreach (var text in cuttexts)
            _seq.Join(text.DOFade(1f, fadeinDuration).SetEase(fadeinEase));

        _seq.OnComplete(() => transparentAll(1f));        
    }

    public override void Close()
    {
        killSequence();

        _seq = DOTween.Sequence();
        foreach (var sprite in cutsprites)
            _seq.Join(sprite.DOFade(0f, fadeoutDuration).SetEase(fadeoutEase));

        foreach (var text in cuttexts)
            _seq.Join(text.DOFade(0f, fadeoutDuration).SetEase(fadeoutEase));

        _seq.OnComplete(() => 
        {
            transparentAll(0f);
            cuttransform.parent.gameObject.SetActive(false);
        });
    }
}
