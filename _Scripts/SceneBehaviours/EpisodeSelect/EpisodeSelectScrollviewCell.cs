using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;


public class EpisodeSelectScrollviewCell : ScrollviewCell<EpisodeSelectScrollviewCellData>
{

    [SerializeField] TextMeshProUGUI DisplayName;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] Image ThumbnailImage;


    private EpisodeSelectScrollviewCellData _celldat;


    public override void SetData(int dataIndex, EpisodeSelectScrollviewCellData data, bool isVertical)
    {
        base.SetData(dataIndex, data, isVertical);

        _celldat = data;

        if (DisplayName != null)
            DisplayName.text = data.DisplayName;        

        if (Description != null)
            Description.text = data.Description;

        if (ThumbnailImage != null && data.Thumbnail != null)
            ThumbnailImage.sprite = data.Thumbnail;            
    }

    public void UOnOpenEpisode()
    {
        if (_celldat == null || _celldat.episode == null) return;

        GlobalData.I.currentChapter = _celldat.chapter;
        GlobalData.I.currentEpisode = _celldat.episode;
        GlobalData.I.currentSequence = null; // 특정 시퀀스로 점프, null 이면 처음부터

        GlobalData.I.journeyProfile.Reset();
        GlobalData.I.journeyProfile.AddLog(LogTypes.EP_START, new SerializableSortedDictionary<string, string>() 
        {
            { LogKeys.EpisodeKey, _celldat.episode.Alias },
            { LogKeys.EpisodeDescription, _celldat.episode.Description }
        });

        GlobalEvent.I.LoadSceneEpisode();
    }

}
