
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Flexalon;
using Lean.Common;
using CustomInspector;
using NodeCanvas.StateMachines;


public class EpisodeControl : MonoBehaviour
{

    [Space(20)] [HideField] public bool _s0;
    [HorizontalLine("에피소드 관련"),HideField] public bool _l0;

    [SerializeField] EpisodeProfile episodeProfile;


    [Space(10)] [HideField] public bool _s2;
    [HorizontalLine("시퀀스 관련"),HideField] public bool _l2;

    [SerializeField] FlexalonConstraint sequencePrefab;

    [HorizontalLine]
    [SerializeField] GameObject sequenceConstraint;
    [SerializeField] LeanPlane sequencePlane;
    
    [HorizontalLine]
    [SerializeField] SpriteRenderer gradientBg;



    private FlexalonConstraint _sequenceLayout;

    private FSMOwner _fsmowner;



    [Space(20)] [HideField,SerializeField] bool _s3;



    private void Awake() 
    {
        _fsmowner = GetComponent<FSMOwner>();
        if (_fsmowner == null)
        {
            Debug.LogError("EpisodeControl ERROR ] FSMOwner 없음");
            return;
        }
    }

    private void OnEnable()
    {
        EpisodeEvent.I.eventSequenceCreate += oneventSequenceCreate;
        EpisodeEvent.I.eventSequenceDestroy += oneventSequenceDestroy;
        EpisodeEvent.I.eventSequenceJumpto += oneventSequenceJumpto;
    }

    private void OnDisable()
    {
        EpisodeEvent.I.eventSequenceCreate -= oneventSequenceCreate;
        EpisodeEvent.I.eventSequenceDestroy -= oneventSequenceDestroy;
        EpisodeEvent.I.eventSequenceJumpto -= oneventSequenceJumpto;
    }

    private IEnumerator Start()
    {
        if (GlobalData.I.currentEpisode != null)
            episodeProfile = GlobalData.I.currentEpisode;

        yield return new WaitForEndOfFrame();

        _fsmowner.behaviour = episodeProfile.EpisodeFSM; 
        _fsmowner.StartBehaviour();

        yield return new WaitForEndOfFrame();

        if (string.IsNullOrEmpty(GlobalData.I.currentSequence) == false)
            JumpToSequence(GlobalData.I.currentSequence);             
    }


    private void oneventSequenceCreate(SequenceProfile profile)
    {
        CreateSequence(profile);
    }

    private void oneventSequenceDestroy() 
    {
        RemoveSequence();
    }

    private void oneventSequenceJumpto(string sequencename)
    {
        RemoveSequence();
        
        JumpToSequence(sequencename);
    }

    private float _maxWidth = 0f;
    private float _totalHeight = 0f;

    public void CreateSequence(SequenceProfile profile)
    {
        if (sequencePrefab == null || profile == null)
        {
            Debug.LogError("SequenceControl ERROR ] sequencePrefab 이나 profile 없음");
            return;
        }

        
        bool deletedPrev = false;
        if (_sequenceLayout != null)
            deletedPrev = false;
        else 
        {            
            // 시퀀스를 만든다
            _sequenceLayout = Instantiate(sequencePrefab, transform);
            _sequenceLayout.name = profile.Alias;
            _sequenceLayout.Target = sequenceConstraint;

            deletedPrev = true;
            _totalHeight = 0f;
            _maxWidth = 0f;
        }
        

        // 컷들을 만든다        
        List<Cut> cuts = new List<Cut>();
        
        foreach(Cut cut in profile.cuts)
        {
            Cut cloned = Instantiate(cut, _sequenceLayout.transform);

            //cloned.AdjustColliderSize();
            cloned.index = cuts.Count;
            cloned.alias = cut.name;
            cuts.Add(cloned);

            if (cloned.boxcollider.size.x >= _maxWidth)
                _maxWidth = cloned.boxcollider.size.x;

            _totalHeight += cloned.boxcollider.size.y;
        }
        

        // 카메라 제한 영역 만든다
        sequencePlane.MinX = _maxWidth * -0.5f;
        sequencePlane.MaxX = _maxWidth * 0.5f;
        sequencePlane.MinY = -_totalHeight - 0.5f;
        sequencePlane.MaxY = -0.5f;


        // 그라디언트 배경
        if (profile.gradientBg == null)
            gradientBg.gameObject.SetActive(false);
        else
        {            
            gradientBg.gameObject.SetActive(true);
            gradientBg.sprite = profile.gradientBg;
            Vector2 spriteSize = profile.gradientBg.bounds.size;

            float scaleX = _maxWidth / spriteSize.x;
            float scaleY = _totalHeight / spriteSize.y;

            gradientBg.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            gradientBg.transform.localPosition = new Vector3(0f, -_totalHeight / 2f - 0.5f, 0);
        }        

        profile.PanelSize = new Vector2(_maxWidth, _totalHeight);

        EpisodeEvent.I.TriggerCutsCreated(profile, cuts, deletedPrev);
    }

    public void RemoveSequence()
    {
        if (_sequenceLayout != null && _sequenceLayout.gameObject != null)
            Destroy(_sequenceLayout.gameObject);

        _sequenceLayout = null;
    }

    public void JumpToSequence(string statename)
    {
        if (GetSequence(statename, out var state) == false)
            return;

        RemoveSequence();
                
        _fsmowner.TriggerState(statename);
    }

    public bool GetSequence(string statename, out FSMState state)
    {
        if (_fsmowner == null)
        {
            Debug.LogWarning("FSMOwner is not assigned!");
            state = null;
            return false;
        }

        var stateMachine = _fsmowner.graph as FSM;
        if (stateMachine == null)
        {
            Debug.LogWarning("StateMachine is not found in the FSMOwner!");
            state = null;
            return false;
        }

        state = stateMachine.GetStateWithName(statename);
        if (state == null)
        {
            Debug.LogError("State named " + statename + " NOT FOUND in the State Machine.");
            return false;
        }

        return true;
    }

}
