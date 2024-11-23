

/// 
/// Enums
/// 

public enum SCENE_TYPE
{
    EPISODESELECT,
    EPISODE,
    JOURNEYSELECT
}

public enum POPUP_TYPE 
{
    PUZZLE    
}



//
// LOG 관련
//

public enum LogTypes
{
    EP_START,
    EP_SEQUENCE,
    EP_SELECT,
    EP_ENDINGCARD
}

public static class LogKeys
{
    public const string ChapterKey = "CH_KEY";

    public const string EpisodeKey = "EP_KEY";
    public const string EpisodeDisplayName = "EP_DISPLAY";
    public const string EpisodeDescription = "EP_DESC";
    
    public const string SequenceKey = "SQ_KEY";
    public const string SequenceDisplayName = "SQ_DISPLAY";
    public const string SequenceDescription = "SQ_DESC";

    public const string SelectNumber = "SEL_NUM";
    public const string SelectQuestion = "SEL_QUESTION";
    public const string SelectAnswer = "SEL_ANSWER";

    public const string EndingCardKey = "END_KEY";
}
