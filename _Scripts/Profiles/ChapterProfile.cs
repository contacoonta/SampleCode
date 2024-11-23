

using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


[CreateAssetMenu(fileName = "ChapterProfile", menuName = "THING/Profile/ChapterProfile")]
public class ChapterProfile : ScriptableObject
{

    [Space(30)] [HideField] public bool _s0;
    [HorizontalLine("챕터 내용"),HideField] public bool _l0;

    public string alias;
    public string displayName;
    public string description;
    [Foldout] public List<EpisodeProfile> episodeProfiles;
}
