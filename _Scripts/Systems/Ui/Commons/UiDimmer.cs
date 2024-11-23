using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UiDimmer : MonoBehaviour
{
    [Space(10)]
    public Image imgDimmer;
    public float alpha = 0.65f;

    [Space]
    public float durationIn = 0.15f;
    public Ease easeIn = Ease.OutSine;
    [Space]
    public float durationOut = 0.35f;
    public Ease easeOut = Ease.InQuad;

    public void Set(float a, float induration = 0.15f, Ease inease = Ease.OutSine, float outduration = 0.35f, Ease outease = Ease.InQuad)
    {
        alpha = a;
        durationIn = induration;
        easeIn = inease;
        durationOut = outduration;
        easeOut = outease;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            FadeIn();
        else if (Input.GetKeyUp(KeyCode.Space))
            FadeOut();
    }

    
    public void FadeIn(Action oncomplete = null)
    {
        imgDimmer.DOFade(alpha, durationIn)
            .SetEase(easeIn)
            .OnComplete(() =>oncomplete?.Invoke());
    }
    
    public void FadeOut(Action oncomplete = null)
    {
        imgDimmer.DOFade(0f, durationOut)
            .SetEase(easeOut)
            .OnComplete(()=>oncomplete?.Invoke());
    }
}