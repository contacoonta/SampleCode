
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public abstract class ACutPattern : ScriptableObject
{
    protected Transform cuttransform;
    protected List<SpriteRenderer> cutsprites;
    protected List<TextMeshPro> cuttexts;

    public void Set(Transform transform, List<SpriteRenderer> sprites = null, List<TextMeshPro> texts = null)
    {
        cuttransform = transform;
        cutsprites = sprites;
        cuttexts = texts;

        OnSet();
    }

    protected abstract void OnSet();
    public abstract void Open();
    public abstract void Close();    
}
