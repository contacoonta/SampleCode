using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;



[CreateAssetMenu(fileName = "SequenceProfile", menuName = "THING/Profile/SequenceProfile")]
public class SequenceProfile : ScriptableObject
{

    [Space(30)] [HideField] public bool _s0;
    [HorizontalLine("시퀀스"),HideField] public bool _l0;
    
    public string Alias;
    public string DisplayName;
    public string Description;
    [Preview(Size.big)] public Sprite Thumbnail;


    [Space(30)] [HideField] public bool _s1;
    [HorizontalLine("컷"),HideField] public bool _l1;
    public Sprite gradientBg;

    [HorizontalLine,HideField] public bool _l2;
    public List<Cut> cuts;



    [Space(30)] [HideField] public bool _s3;
    [HorizontalLine("이벤트 (시퀀스 끝에 발동))"),HideField] public bool _l3;        
    
    [Foldout, SerializeField] public ASequenceEvent sequenceevent;


    [Space(50)] [HorizontalLine,HideField] public bool _l4;
    [ReadOnly] public Vector2 PanelSize;
    

}
