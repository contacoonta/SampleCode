


public class UiPopupBackToMenu : UiPopup
{
    
    

    public override void OnOpened(object obj) 
    {        
    }

    public override void OnClosed() 
    {        
    }


    public void ToEpisodeSelect()
    {
        GlobalEvent.I.LoadSceneEpisodeSelect();
    }

    
}
