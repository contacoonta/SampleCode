using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class IntroLogo : MonoBehaviour
{

    [Space(10)]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform logoTransform;
    [SerializeField] UnityEvent onFadeComplete;

    private Sequence fadeSequence; 

    private void Awake() 
    {
        canvasGroup.alpha = 0;
        logoTransform.localScale = Vector3.one;
    }

    private void Start()
    {
        if (GlobalSetting.I.Intro == false)
        {
            onFadeComplete?.Invoke();
            return;
        }

        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutSine));
        fadeSequence.Join(logoTransform.DOScale(Vector3.one*1.1f, 2f));
        fadeSequence.Append(canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InSine));
        fadeSequence.Join(logoTransform.DOScale(Vector3.one*1.125f, 0.5f));
        fadeSequence.OnComplete(() => onFadeComplete?.Invoke());
    }

    public void CancelFade()
    {
        if (fadeSequence != null && fadeSequence.IsActive())
        {
            fadeSequence.Kill();
            Debug.Log("Fade sequence canceled manually.");
        }
    }

    void OnDisable()
    {
        CancelFade();
    }

    void OnDestroy()
    {
        CancelFade();
    }
}