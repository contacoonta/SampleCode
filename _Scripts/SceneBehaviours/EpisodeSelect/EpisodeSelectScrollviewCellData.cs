
using UnityEngine;

public class EpisodeSelectScrollviewCellData : ScrollviewCellData
{
    public enum CellType { HEAD, BODY, TAIL}

    public CellType cellType;
    public string DisplayName;
    public string Description;
    public Sprite Thumbnail;
    public ChapterProfile chapter;
    public EpisodeProfile episode;
}
