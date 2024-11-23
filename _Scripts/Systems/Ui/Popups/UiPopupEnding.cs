

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CustomInspector;
using DG.Tweening;

public class UiPopupEnding : UiPopup
{
    
    [Space(30)]
    [HorizontalLine]
    [SerializeField] GameObject cardHidden;
    [SerializeField] GameObject cardResult;

    
    [SerializeField] CanvasGroup JourneyCanvas;
    [Foldout, SerializeField] AUiPattern JourneyPattern;


    private SequenceEventEnding _ending;

    public override void OnOpened(object obj) 
    {
        if (obj is SequenceProfile profile == false)
            return;

        if (profile.sequenceevent is SequenceEventEnding == false)
            return;

        _ending = (profile.sequenceevent as SequenceEventEnding);

        cardHidden.SetActive(true);
        cardResult.SetActive(false);

        JourneyCanvas.alpha = 0f;
        JourneyPattern.Set(null, JourneyCanvas.GetComponent<RectTransform>(), JourneyCanvas);
    }


    public override void OnClosed() 
    {        
    }


    public void ShowEnding()
    {
        if (_ending == null)
        {
            Debug.LogError("UiPopupEnding ERROR ] 엔딩 카드 없음");
            return;
        }
        
        cardResult.SetActive(false);
        cardResult.GetComponent<Image>().sprite = _ending.endingSprite;
        cardResult.transform.localScale = new Vector3(0, 1, 1);

        // 엔딩카드 뒤집히는 연출
        Sequence cardSequence = DOTween.Sequence();
        cardSequence.Append(cardHidden.transform.DOScaleX(0.0f, 0.2f).SetEase(Ease.InQuad));
        cardSequence.AppendCallback(() => {cardResult.SetActive(true);});
        cardSequence.Append(cardResult.transform.DOScaleX(1, 0.25f).SetEase(Ease.OutBack));
        cardSequence.Join(cardHidden.transform.DOScaleX(0, 0.2f).SetEase(Ease.InQuad));
        cardSequence.OnComplete(() => 
        {
            cardHidden.SetActive(false);
            ShowJourneyButton();

            GlobalData.I.journeyProfile.AddLog(LogTypes.EP_ENDINGCARD, new SerializableSortedDictionary<string, string>()
            {
                { LogKeys.EndingCardKey, _ending.endingSprite.name }
            });
        });
    }
    
    
    public void ShowJourneyButton()
    {
        JourneyPattern.Open();
    }


    public void ToJourneyPage()
    {
        GlobalEvent.I.LoadSceneJourneySelect();
    }
}
