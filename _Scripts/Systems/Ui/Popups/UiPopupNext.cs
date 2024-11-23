

public class UiPopupNext : UiPopup
{

    public override void OnOpened(object obj) 
    {
        if (obj is SequenceProfile profile == false)
            return;
    }


    public override void OnClosed() 
    {
        
    }

    public void SelectNext()
    {
        EpisodeEvent.I.TriggerSelectedNext(null, 0, null);
    }
}
