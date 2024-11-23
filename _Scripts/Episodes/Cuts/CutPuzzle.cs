using UnityEngine;
using CustomInspector;

public class CutPuzzle : Cut
{

    [HorizontalLine("인터랙션 - Drag & Drop")][HideField,SerializeField] bool _l11;    

    [ReadOnly] public Transform before;
    [ReadOnly] public Transform after;
    [ReadOnly] public Transform interaction;


    private bool iscomplete = false;

    override protected void OnValidate() 
    {
        base.OnValidate();
        
        before = transform.FindDeepChild("_BEFORE_");        
        after = transform.FindDeepChild("_AFTER_");
        interaction = transform.FindDeepChild("_INTERACTION_");
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        before = transform.FindDeepChild("_BEFORE_");
        if (before == null)
            Debug.LogError("Cut Puzzle ERROR ] _BEFORE_ 없음");

        after = transform.FindDeepChild("_AFTER_");
        if (after == null)
            Debug.LogError("Cut Puzzle ERROR ] _AFTER_ 없음");

        interaction = transform.FindDeepChild("_INTERACTION_");
        if (interaction == null)
            Debug.LogError("Cut Puzzle ERROR ] _INTERACTION_ 없음");

        interaction?.gameObject.SetActive(false);

        updatebeforeafter();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //EpisodeEvent.I.eventCutActived += oneventCutActived;
        EpisodeEvent.I.eventPuzzleComplete += oneventPuzzleComplete;

        interaction?.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //EpisodeEvent.I.eventCutActived -= oneventCutActived;
        EpisodeEvent.I.eventPuzzleComplete -= oneventPuzzleComplete;
    }

    private void oneventCutActived(Cut cut)
    {
        if (cut != this) return;        

        GlobalEvent.I.TriggerPopupOpen(POPUP_TYPE.PUZZLE, (object)interaction.gameObject);
    }


    private void oneventPuzzleComplete()
    {
        iscomplete = true;

        updatebeforeafter();

        GlobalEvent.I.TriggerPopupClose();
    }


    protected override void OnOpened(object obj = null)
    {
        base.OnOpened(obj);    

        updatebeforeafter();    
    }

    private void updatebeforeafter()
    {
        if (before == null || after == null) return;

        before.gameObject.SetActive(!iscomplete);
        after.gameObject.SetActive(iscomplete);
    }
    
}