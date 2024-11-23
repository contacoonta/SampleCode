using UnityEngine;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;


public class EpisodeSelectScrollviewControl : ScrollviewControl<EpisodeSelectScrollviewCellData, EpisodeSelectScrollviewCell>
{
    [SerializeField] protected EnhancedScrollerCellView         cellviewHeader;
    [SerializeField] protected EnhancedScrollerCellView         cellviewBody;
    [SerializeField] protected EnhancedScrollerCellView         cellviewTail;
       
    override public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        EpisodeSelectScrollviewCell cellView = null;

        if (_datalist[dataIndex].cellType == EpisodeSelectScrollviewCellData.CellType.BODY)
            cellView = scroller.GetCellView(cellviewBody) as EpisodeSelectScrollviewCell;
        else if (_datalist[dataIndex].cellType == EpisodeSelectScrollviewCellData.CellType.HEAD)
            cellView = scroller.GetCellView(cellviewHeader) as EpisodeSelectScrollviewCell;
        else if (_datalist[dataIndex].cellType == EpisodeSelectScrollviewCellData.CellType.TAIL)
            cellView = scroller.GetCellView(cellviewTail) as EpisodeSelectScrollviewCell;        

        cellView.name = "Cell " + _datalist[dataIndex].alias;
        cellView.onactionSelected = CellViewSelected;
        cellView.SetData(dataIndex, _datalist[dataIndex], (this.scroller == scroller));
        
        return cellView;
    }
    
}
