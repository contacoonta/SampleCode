using UnityEngine;
using Lean.Touch;
using Lean.Common;



[RequireComponent(typeof(LeanDragCamera), typeof(LeanConstrainToOrthographic), typeof(LeanMouseWheel))]
public class EpisodeCameraEffect : MonoBehaviour
{
    private LeanDragCamera leancamera;
    private LeanConstrainToOrthographic leanconstrain;
    private LeanMouseWheel leanmouse;

    private void Awake()
    {
        leancamera = GetComponent<LeanDragCamera>();
        leanconstrain = GetComponent<LeanConstrainToOrthographic>();
        leanmouse = GetComponent<LeanMouseWheel>(); 
    }


    private void OnEnable()
    {
        GlobalEvent.I.eventPopupOpened += oneventPopupOpened;
        GlobalEvent.I.eventPopupClosed += oneventPopupClosed;
    }

    private void OnDisable() 
    {
        GlobalEvent.I.eventPopupOpened -= oneventPopupOpened;
        GlobalEvent.I.eventPopupClosed -= oneventPopupClosed;
    }



    private void oneventPopupOpened(POPUP_TYPE popuptype, object obj)
    {
        leancamera.enabled = false;
        leanconstrain.enabled = false;
        leanmouse.enabled = false;
    }

    private void oneventPopupClosed()
    {
        leancamera.enabled = true;
        leanconstrain.enabled = true;
        leanmouse.enabled = true;
    }


    
}
