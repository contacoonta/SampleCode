
using UnityEngine;
using EnhancedUI.EnhancedScroller;


public class JourneySelectScrollviewControl : ScrollviewControl<JourneySelectScrollviewCellData, JourneySelectScrollviewCell>
{
    [SerializeField] protected EnhancedScrollerCellView         cellviewJourneyStart;
    [SerializeField] protected EnhancedScrollerCellView         cellviewJourneySequence;
    [SerializeField] protected EnhancedScrollerCellView         cellviewJourneySelect;
    [SerializeField] protected EnhancedScrollerCellView         cellviewJourneyEndingCard;    
    

    override public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        JourneySelectScrollviewCell cellView = null;

        if (_datalist[dataIndex].cellType == LogTypes.EP_START)
            cellView = scroller.GetCellView(cellviewJourneyStart) as JourneySelectScrollviewCell;
        else if (_datalist[dataIndex].cellType == LogTypes.EP_SEQUENCE)
            cellView = scroller.GetCellView(cellviewJourneySequence) as JourneySelectScrollviewCell;
        else if (_datalist[dataIndex].cellType == LogTypes.EP_SELECT)
            cellView = scroller.GetCellView(cellviewJourneySelect) as JourneySelectScrollviewCell;
        else if (_datalist[dataIndex].cellType == LogTypes.EP_ENDINGCARD)
            cellView = scroller.GetCellView(cellviewJourneyEndingCard) as JourneySelectScrollviewCell;
        else
            return null;

        cellView.name = "Cell " + _datalist[dataIndex].alias;
        cellView.onactionSelected = CellViewSelected;
        cellView.SetData(dataIndex, _datalist[dataIndex], (this.scroller == scroller));
        
        return cellView;
    }
    
}
