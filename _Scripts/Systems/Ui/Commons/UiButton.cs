using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class UiButton : MonoBehaviour, IPointerClickHandler
{

    [Space]
    public TextMeshProUGUI title;

    [Space]
    [SerializeField] UnityEvent Clicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScaleY(.9f, 0.1f));
        sequence.Insert(.0f, transform.DOScaleX(1.1f, 0.05f));
        sequence.Append(transform.DOScale(1f, 0.1f));
        sequence.AppendInterval(0.05f);
        sequence.AppendCallback(() => Clicked.Invoke());
    }
}