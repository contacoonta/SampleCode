
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;



[CreateAssetMenu(menuName = "THING/Data/Container/ChapterContainer")]
public class ChapterContainerData : ScriptableObject
{    
    public List<ChapterProfile> Chapters => chapters;
    [Foldout, SerializeField] List<ChapterProfile> chapters = new List<ChapterProfile>();


    public void Clear()
    {
        chapters.Clear();
    }
        

    public ChapterProfile GetChapterProfile(string chapterAlias)
    {
        return null;
    }

    public EpisodeProfile GetEpisodeProfile(string episodeAlias)    
    {
        return null;
    }

}