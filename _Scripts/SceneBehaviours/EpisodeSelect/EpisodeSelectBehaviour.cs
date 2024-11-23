
using System.Collections;
using UnityEngine;


public class EpisodeSelectBehaviour : MonoBehaviour
{        
    
    [SerializeField] EpisodeSelectScrollviewControl episodeview;

    private IEnumerator Start() 
    {
        yield return new WaitForEndOfFrame();

        foreach( var ch in GlobalData.I.chapterContainer.Chapters )
        {
            episodeview.AddCell(new EpisodeSelectScrollviewCellData() {cellType = EpisodeSelectScrollviewCellData.CellType.HEAD, alias = ch.alias, height = 400f, DisplayName = ch.displayName, Description = ch.description });

            foreach( var ep in ch.episodeProfiles)
                episodeview.AddCell(new EpisodeSelectScrollviewCellData() 
                {
                    cellType = EpisodeSelectScrollviewCellData.CellType.BODY, 
                    alias = ep.Alias, 
                    height = 1200f, 
                    DisplayName = ep.DisplayName, 
                    Description = ep.Description, 
                    Thumbnail = ep.Thumbnail, 
                    chapter = ch, 
                    episode = ep 
                });

            episodeview.AddCell(new EpisodeSelectScrollviewCellData() {cellType = EpisodeSelectScrollviewCellData.CellType.TAIL, alias = "tail", height = 100f});
        }
    }

}
