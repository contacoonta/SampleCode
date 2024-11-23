using UnityEngine;
using Lean.Touch;

[CreateAssetMenu(menuName = "THING/Cut/Abilities/BlockScrollAnimation")]
public class AbilityBlockScrollAnimation : AAbility
{
    [Space(20), Tooltip("터치 드래그에 따른 보간 속도 (클수록 빠름)")]
    [SerializeField, Range(1f, 10f)] float animationSpeed = 5f;
    private float _animationPercent = 0f;
    private float _animationLength = 0f;


    private LeanDragCamera _leancamera;
    private Cut _cut;
    private Animator _animator;

    private bool _isDragEnabled = false;
    private float _targetDragValue = 0f;


#region _ABILITY_

    public override void Init()
    {
        if (_cut == null || _animator == null) return;

        _animationPercent = _cut.Value;

        ScrollAnimate();
    }

    public override void Execute(EpisodeCamera epcam, Cut cut)
    {
        if (cut == null)
        {
            Debug.LogWarning("Cut | Animator 없음");
            return;
        }
        else
        {
            _cut = cut;
            _animator = cut.GetComponent<Animator>();
            _animator.speed = 0;            
            _animationLength = GetCurrentAnimationLength();
            _animationPercent = _cut.Value;
        }

        if (epcam == null)
        {
            Debug.LogWarning("EpisodeCamera 없음");
            return;
        }

        _leancamera = epcam.GetComponent<LeanDragCamera>();
        if (_leancamera == null)
        {
            Debug.LogWarning("LeanDragCamera 없음");
            return;
        }

        epcam.MoveCameraToCut(cut);

        _leancamera.enabled = false;
        _isDragEnabled = false;

        if (_cut.Value <= 0.5f)
            _targetDragValue = 1.0f;
        else
            _targetDragValue = 0.0f;

        LeanTouch.OnFingerUpdate += OnFingerUpdate;
    }

    override public void Tick()
    {
        if (_cut == null || _animator == null) return;

        ScrollAnimate();
    }

#endregion _ABILITY_

#region _DRAG_

    void OnFingerUpdate(LeanFinger finger)
    {
        if (_isDragEnabled || _cut == null) return;

        float screenDiagonal = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
        float deltaMovement = Vector2.Distance(finger.LastScreenPosition, finger.ScreenPosition) / screenDiagonal;
        Vector2 dragDirection = finger.ScreenPosition - finger.LastScreenPosition;

        // 애니메이션의 길이가 길어질수록 deltaMovement를 줄임
        float adjustedDeltaMovement = deltaMovement * animationSpeed / _animationLength;

        if (dragDirection.y > 0) // 위로 드래그
            _cut.Value += adjustedDeltaMovement;
        else if (dragDirection.y < 0) // 아래로 드래그
            _cut.Value -= adjustedDeltaMovement;

        _cut.Value = Mathf.Clamp(_cut.Value, 0f, 1f);

        float dragDifference = Mathf.Abs(_targetDragValue - _cut.Value);

        // 누적된 드래그 거리가 임계값에 도달했는지 확인
        if (dragDifference == 0f)
        {
            _leancamera.enabled = true;
            _isDragEnabled = true;

            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }
    }

#endregion _DRAG_

#region _ANIMATION_

    public void ScrollAnimate()
    {
        if (_animator == null) return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        _animationPercent = Mathf.Lerp(_animationPercent, _cut.Value, Time.deltaTime * 5f);
        _animator.Play(stateInfo.fullPathHash, 0, _animationPercent);        
    }

    private float GetCurrentAnimationLength()
    {
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            return clipInfo[0].clip.length;
        }
        return 1f; // 기본값
    }

#endregion _ANIMATION_
}