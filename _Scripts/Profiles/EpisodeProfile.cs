

using UnityEngine;
using CustomInspector;
using NodeCanvas.StateMachines;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "EpisodeProfile", menuName = "THING/Profile/EpisodeProfile")]
public class EpisodeProfile : ScriptableObject
{

    [Space(30)] [HideField] public bool _s0;
    [HorizontalLine("에피소드 내용"),HideField] public bool _l0;

    public string Alias;
    public string DisplayName;
    public string Description;
    public FSM EpisodeFSM;

    [Preview(Size.big)] public Sprite Thumbnail;

    
}
