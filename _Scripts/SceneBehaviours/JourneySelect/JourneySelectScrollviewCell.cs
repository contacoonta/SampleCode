using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class JourneySelectScrollviewCell : ScrollviewCell<JourneySelectScrollviewCellData>
{

    [SerializeField] TextMeshProUGUI DisplayName;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] Image ThumbnailImage;

    [SerializeField] SpriteContainer spriteContainer;


    private JourneySelectScrollviewCellData _celldat;


    public override void SetData(int dataIndex, JourneySelectScrollviewCellData data, bool isVertical)
    {
        base.SetData(dataIndex, data, isVertical);

        _celldat = data;

        if (DisplayName != null)
            DisplayName.text = data.DisplayName;        

        if (Description != null)
            Description.text = data.Description;

        if (ThumbnailImage != null && spriteContainer != null)
            if (spriteContainer.TryGetAsset(data.ThumbnailKey, out var sprite))
                ThumbnailImage.sprite = sprite;
    }

    public void UOnOpenJourney()
    {
        if (_celldat == null) return;

        GlobalData.I.currentSequence = _celldat.SequenceKey;
                
        GlobalEvent.I.LoadSceneEpisode();
    }

}
