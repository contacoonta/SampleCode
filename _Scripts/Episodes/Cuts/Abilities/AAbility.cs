
using UnityEngine;

public abstract class AAbility : ScriptableObject
{     
    public abstract void Init();
    public abstract void Execute(EpisodeCamera epcam, Cut cut);
    public abstract void Tick();
}