using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using CustomInspector;
using DG.Tweening;
using Lean.Common;



[RequireComponent(typeof(LeanConstrainToOrthographic))]
public class EpisodeCamera : MonoBehaviour
{
    
    [Space(10)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("카메라 설정")][HideField,SerializeField] bool _l0;

    [Tooltip("카메라 움직임 연산의 임계값 (클수록 둔감)")]
    [SerializeField] float updatePosThreshold = 0.5f;


    [Space] [HideField,SerializeField] bool _s1;
    [HorizontalLine][HideField,SerializeField] bool _l1;


    [Tooltip("카메라 영역을 포함한, 바닥Y 월드좌표")]
    [SerializeField, ReadOnly] float BottomYCamera;
    
    public float BottomYSequence => leanconstrain != null ? leanconstrain.Plane.MinY : 0f;
    public bool IsBottom => BottomYCamera <= BottomYSequence;

    
    private Camera _camera;
    private List<Cut> _cuts;
    private SequenceProfile _profile;
    private LeanConstrainToOrthographic leanconstrain;


    [Space(30)] [HideField,SerializeField] bool _s2;

    

    private CancellationTokenSource _cts;

    private void Awake()
    {
        leanconstrain = GetComponent<LeanConstrainToOrthographic>();
    }

    private void OnEnable()
    {
        _camera = GetComponent<Camera>();

        EpisodeEvent.I.eventCutsCreated += OneventCutsCreated;
        EpisodeEvent.I.eventSequenceReachedBottom += OneventSequenceReachedBottom;
    }

    private void OnDisable() 
    {
        EpisodeEvent.I.eventCutsCreated -= OneventCutsCreated;
        EpisodeEvent.I.eventSequenceReachedBottom -= OneventSequenceReachedBottom;
    }



    private void OneventCutsCreated(SequenceProfile profile, List<Cut> cuts, bool deletedPrev)
    {
        ResetCamera(profile, cuts, deletedPrev);
    }

    private void OneventSequenceReachedBottom(bool bottom, SequenceProfile profile)
    {
        _triggeredBottom = bottom;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.25f);
        
        _cts = new CancellationTokenSource();
        UpdateAsync(_cts.Token).Forget();
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }


    public void ResetCamera(SequenceProfile profile, List<Cut> cuts, bool deletedPrev)
    {
        _profile = profile;

        float aspect = (float)Screen.width / (float)Screen.height;
        float orthographicsize = (profile.PanelSize.x / 2) / aspect;
        _camera.orthographicSize = orthographicsize;
        if ( deletedPrev)
            _cuts = cuts;
        else
            _cuts.AddRange(cuts);
        
        MoveCameraToTop(deletedPrev);
    }

    public void MoveCameraToTop(bool deletedPrev)
    {
        if (_cuts == null || _cuts.Count == 0 || _cts == null) return;

        if (deletedPrev)
        {
            Cut topcut = _cuts[0];
            float spriteHeight = topcut.boxcollider.size.y;
            float spriteTopY = topcut.transform.position.y + spriteHeight / 2;

            Vector3 newPosition = _camera.transform.position;
            newPosition.y = spriteTopY;
            _camera.transform.position = newPosition;
        }

        EpisodeEvent.I.TriggerSequenceReachedBottom(false, _profile);

        SetAdjacentSpritesActiveAsync(0);        
    }

    public void MoveCameraToCut(Cut cut, float duration = 0.5f)
    {
        BoxCollider2D collider = cut.GetComponent<BoxCollider2D>();

        if (collider == null)
        {
            Debug.LogError("Cut object does not have a BoxCollider2D component.");
            return;
        }

        Vector2 center = collider.bounds.center;
        _camera.transform.DOMoveY(center.y, duration).SetEase(Ease.OutSine);
    }



    private float _worldposprev;
    private bool _triggeredBottom = false;

    private async UniTaskVoid UpdateAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await UniTask.Yield(PlayerLoopTiming.Update);

                if (_cuts == null || _cuts.Count <= 0) 
                    continue;

                if (_camera == null)
                    return;

                Vector3 worldpos = _camera.transform.position;
                BottomYCamera = worldpos.y - _camera.orthographicSize;

                if (IsBottom && _triggeredBottom == false)
                    EpisodeEvent.I.TriggerSequenceReachedBottom(true, _profile);

                if (Mathf.Abs(worldpos.y - _worldposprev) < updatePosThreshold)
                    continue;

                _worldposprev = worldpos.y;

                DetectHittedAndFocusedCut();           
            }
            catch (OperationCanceledException)
            {
                Debug.Log("취소 종료");
                break;
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred in UpdateAsync: {e.Message}");
            }
        }
    }


    


    private float focusThreshold = 5.0f;
    private int _idxprev = -2;
    private void DetectHittedAndFocusedCut()
    {
        Vector3 cameraCenter = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit2D hit = Physics2D.Raycast(cameraCenter, Vector2.zero);

        if (hit.collider == null) return;

        Cut hittedCut = hit.collider.GetComponent<Cut>();
        if (hittedCut == null) return;

        int index = _cuts.IndexOf(hittedCut);
        if (index == -1 || index == _idxprev) return;        

        // DetectFocusedCut logic
        Bounds bounds = hit.collider.bounds;
        float colliderCenterY = bounds.center.y;
        float diff = colliderCenterY - cameraCenter.y;

        if (Mathf.Abs(diff) <= focusThreshold)
        {
            _idxprev = index;
            
            SetAdjacentSpritesActiveAsync(index);

            EpisodeEvent.I.TriggerCutFocused(this, hittedCut);
        }
    }
    

    // 현재 컷의 위-아래 활성화 , 나머지 비활성화
    private void SetAdjacentSpritesActiveAsync(int currentIndex)
    {        
        for (int i = 0; i < _cuts.Count; i++)
        {
            bool shouldBeActive = Mathf.Abs(i - currentIndex) <= 1;
            if (_cuts[i] != null && _cuts[i].gameObject != null)
            {
                if (_cuts[i].gameObject.activeSelf != shouldBeActive)
                {
                    Cut cut = _cuts[i].GetComponent<Cut>();
                    if (cut != null)
                    {
                        if (shouldBeActive == false)
                            cut.Close();
                        else
                            cut.gameObject.SetActive(shouldBeActive);
                    }
                }
            }
        }       
    }
}