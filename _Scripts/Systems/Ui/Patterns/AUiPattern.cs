using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AUiPattern : ScriptableObject
{

    protected UiDimmer uidimmer;
    protected RectTransform uipanel;
    protected CanvasGroup uicanvas;

    
    public void Set(UiDimmer dim, RectTransform panel, CanvasGroup canvas )
    {
        uidimmer = dim;
        uipanel = panel;
        uicanvas = canvas;
    }

    public abstract void Open();
    public abstract void Close();    
    
}
