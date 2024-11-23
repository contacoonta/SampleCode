
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using TMPro;

public class UiPopupSelect : UiPopup
{

    [Space(20)] [HideField,SerializeField] bool _s11;
    [HorizontalLine][HideField,SerializeField] bool _l11;
    public TextMeshProUGUI tmquestion;
    public List<UiButton> buttons = new List<UiButton>();

    private SequenceEventSelect _eventselect;

    public override void OnOpened(object obj) 
    {
        if (obj is SequenceProfile profile == false)
            return;

        if (profile.sequenceevent is SequenceEventSelect == false)
            return;


        _eventselect = (profile.sequenceevent as SequenceEventSelect);
        
        tmquestion.text = _eventselect.question;

        for( int i=0; i<buttons.Count; i++ )
        {
            buttons[i].gameObject.SetActive(false);

            if (i < _eventselect.selects.Count && !string.IsNullOrEmpty(_eventselect.selects[i]))
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].title.text = _eventselect.selects[i];
            }
        }
    }


    public override void OnClosed() 
    {
        foreach(var btn in buttons)
        {
            btn.title.text = "";
            btn.gameObject.SetActive(false);
        }
    }

    
    public void SelectItem(int num)
    {
        if (_eventselect == null || num < 1 || num > _eventselect.selects.Count )
        {
            Debug.LogWarning("SequenceEventSelect : SelectItem - ERROR");
            return;
        }

        // num : 1 ~ 4 => idx : 0 ~ 3 으로 변환
        int idx = num - 1;
        EpisodeEvent.I.TriggerSelectedNext(_eventselect.question, num, _eventselect.selects[idx]);
    }
}
