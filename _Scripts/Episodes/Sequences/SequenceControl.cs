using System.Collections;
using System.Collections.Generic;
using Flexalon;
using UnityEngine;


public class SequenceControl : MonoBehaviour
{
    [SerializeField] FlexalonConstraint layoutPrefab;

    private FlexalonConstraint _layout;

    public void Create(SequenceProfile profile)
    {
        if (layoutPrefab == null)
        {
            Debug.LogError("SequenceControl ERROR ] layoutPrefab 없음");
            return;
        }

        if (_layout != null)
            Destroy(_layout);
            
        _layout = Instantiate(layoutPrefab, transform.parent);
        _layout.Target = gameObject;

        if ((profile is SequenceProfile p) == false)
        {
            Debug.LogError("SequenceControl Error ] profile 없음");
            return;
        }

        foreach( Cut cut in p.cuts )
        {
            Instantiate(cut,_layout.transform);
        }
    }

    public void RemoveAll()
    {
        Destroy(_layout.gameObject);
        _layout = null;
    }
}
