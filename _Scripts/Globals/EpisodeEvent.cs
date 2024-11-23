
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[AssetPath("EpisodeEvent")]
public class EpisodeEvent : ScriptableSingleton<EpisodeEvent>
{

    public UnityAction<SequenceProfile>         eventSequenceCreate;
    public UnityAction                          eventSequenceDestroy;
    public UnityAction<string>                  eventSequenceJumpto;
    public UnityAction<bool, SequenceProfile>   eventSequenceReachedBottom;

    public UnityAction<SequenceProfile, List<Cut>, bool>    eventCutsCreated;
    public UnityAction<EpisodeCamera, Cut>                  eventCutFocused;

    public UnityAction<AudioClip>                           eventSFXPlay;

    public UnityAction<string, int, string>                 eventSequenceSelectedNext;

    public UnityAction                                      eventPuzzleComplete;



    public void TriggerSequenceCreate(SequenceProfile profile) => eventSequenceCreate?.Invoke(profile);
    public void TriggerSequenceDestroy() => eventSequenceDestroy?.Invoke();
    public void TriggerSequenceReachedBottom(bool bottom, SequenceProfile profile) => eventSequenceReachedBottom?.Invoke(bottom, profile);
    public void TriggerSequenceJumpTo(string sequencename) => eventSequenceJumpto?.Invoke(sequencename);


    public void TriggerCutsCreated(SequenceProfile profile, List<Cut> cuts, bool deletedPrev) => eventCutsCreated?.Invoke(profile, cuts, deletedPrev);    
    public void TriggerCutFocused(EpisodeCamera epcam, Cut cut) => eventCutFocused?.Invoke(epcam, cut);

    public void TriggerSelectedNext(string question, int select, string answer) => eventSequenceSelectedNext?.Invoke(question, select, answer);

    public void TriggerSFXPlay(AudioClip clip) => eventSFXPlay?.Invoke(clip);

    public void TriggerPuzzleComplete() => eventPuzzleComplete?.Invoke();    
}
