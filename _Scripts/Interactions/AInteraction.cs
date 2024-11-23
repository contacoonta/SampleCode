
using UnityEngine;
using CustomInspector;

public abstract class AInteraction : MonoBehaviour
{
    [Space(30)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("Offset(단위: pixel)")][HideField,SerializeField] bool _l0;

    public Vector3 Offset;
    
}
