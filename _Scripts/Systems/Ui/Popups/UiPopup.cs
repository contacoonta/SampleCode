
using UnityEngine;
using CustomInspector;

public class UiPopup : MonoBehaviour
{

    [Space(30)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("UI 패턴")][HideField,SerializeField] bool _l0;
    [Foldout]
    [SerializeField] AUiPattern pattern;
    
    [Space(30)] [HideField,SerializeField] bool _space1;
    [HorizontalLine][HideField,SerializeField] bool _line1;
    [SerializeField] protected UiDimmer dimmer;
    [SerializeField] protected RectTransform panel;


    private bool _isopened = false;
    private void Awake()
    {
        CanvasGroup canvas = panel.GetComponent<CanvasGroup>();
        if (canvas == null)
            Debug.LogError($"UiPopup - CanvasGroup Error");

        if (pattern == null)
            Debug.LogError($"UiPopup - Ui Pattern Error");

        pattern.Set(dimmer,panel,canvas);
    }

    public void Open(object obj = null)
    {
        if (_isopened == true) return;
        _isopened = true;
        
        pattern.Open();
        OnOpened(obj);
    }

    public void Close()
    {
        if (_isopened == false) return;        
        _isopened = false;

        OnClosed();
        pattern.Close();
    }


    public virtual void OnOpened(object obj = null) {}
    public virtual void OnClosed() {}
}
