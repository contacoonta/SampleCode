
using System.Collections;
using UnityEngine;


public class JourneySelectBehaviour : MonoBehaviour
{
    [SerializeField] JourneySelectScrollviewControl journeyview;


    private IEnumerator Start() 
    {
        if (GlobalData.I.journeyProfile.logs.Count <= 0)
            yield break;

        yield return new WaitForEndOfFrame();
        
        foreach( var log in GlobalData.I.journeyProfile.logs )
        {
            var logtype = log.logType;            

            if (logtype == LogTypes.EP_START)
            {
                var celldat = new JourneySelectScrollviewCellData();
                celldat.alias = log.logType.ToString();
                celldat.cellType = logtype;
                celldat.height = 200f;
                celldat.DisplayName = log.GetValue(LogKeys.EpisodeKey);
                celldat.Description = log.GetValue(LogKeys.EpisodeDescription);
                
                journeyview.AddCell(celldat);
            }
            else if (logtype == LogTypes.EP_SEQUENCE)
            {
                var celldat = new JourneySelectScrollviewCellData();
                celldat.alias = log.logType.ToString();
                celldat.cellType = logtype;
                celldat.height = 250f;
                string seqkey = log.GetValue(LogKeys.SequenceKey);
                celldat.DisplayName = seqkey;
                celldat.Description = log.GetValue(LogKeys.SequenceDescription);
                celldat.SequenceKey = seqkey;
                
                journeyview.AddCell(celldat);
            }
            else if (logtype == LogTypes.EP_SELECT)
            {
                var celldat = new JourneySelectScrollviewCellData();
                celldat.alias = log.logType.ToString();
                celldat.cellType = logtype;
                celldat.height = 200f;
                celldat.DisplayName = $"{log.GetValue(LogKeys.SelectQuestion)}";
                celldat.Description = $"<size=80%>선택  [</size> <b> <color=orange>{log.GetValue(LogKeys.SelectNumber)}</color> </b> <size=80%>:</size> <b> <color=orange>{log.GetValue(LogKeys.SelectAnswer)}</color> </b><size=80%>]</size>";
                
                journeyview.AddCell(celldat);
            }
            else if (logtype == LogTypes.EP_ENDINGCARD)
            {
                var celldat = new JourneySelectScrollviewCellData();
                celldat.alias = log.logType.ToString();
                celldat.cellType = logtype;
                celldat.height = 400f;
                celldat.ThumbnailKey = log.GetValue(LogKeys.EndingCardKey);     

                journeyview.AddCell(celldat);           
            }
        }
    }
}
