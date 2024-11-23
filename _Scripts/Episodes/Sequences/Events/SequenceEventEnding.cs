using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "THING/Sequence/Event/Ending")]
public class SequenceEventEnding : ASequenceEvent
{


    [Space(10)] [HideField] public bool _s0;
    [HorizontalLine(color:FixedColor.DustyBlue,message:"엔딩카드"),HideField] public bool _l0;
    
    [Preview(Size.max)]
    public Sprite endingSprite;
    
}
