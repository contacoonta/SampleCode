using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "THING/Sequence/Event/Select")]
public class SequenceEventSelect : ASequenceEvent
{

    [Space(10)] [HideField] public bool _s0;
    [HorizontalLine(color:FixedColor.DustyBlue,message:"선택메뉴 (최대:4)"),HideField] public bool _l0;


    [Space(10)]
    public string question;

    [Space(10)]
    public List<string> selects = new List<string>();
    
}
