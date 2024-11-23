
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;
using TMPro;


public class Cut : MonoBehaviour
{

    [Space(20)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("기본 속성 (1=100px)")][HideField,SerializeField] bool _l0;
    
    [Tooltip("해당 수치(1=100px)만큼 위아래로 높이 조절")]
    public float extraHeight;

    [Tooltip("해당 수치(1=100px)만큼 위치 조절")]
    public Vector2 extraOffset;

    [Tooltip("등장시 애니메이션 연출")]
    [Foldout, SerializeField] ACutPattern pattern;


    [Space(20)] [HideField,SerializeField] bool _s1;
    [HorizontalLine("확장 기능")][HideField,SerializeField] bool _l1;

    [Foldout, SerializeField] AAbility ability;

    

    [Space(20)] [HideField,SerializeField] bool _s2;
    [HorizontalLine][HideField,SerializeField] bool _l2;

    [ReadOnly] public int index = -1;   // 시퀀스가 할당해주는 컷 인덱스
    [ReadOnly] public string alias;     // 시퀀스가 할당해주는 컷 이름    
    [ReadOnly] public bool isfocused = false;
    [SerializeField, ReadOnly] float _value = 0f; // 컷을 컨트롤 하는 0f ~ 1f 값
    public float Value { get => _value; set => _value = value; }

    [Space]
    [ReadOnly] public Transform root;
    [ReadOnly] public BoxCollider2D boxcollider;     // Sprite의 크기




    [Space(50)] [HideField,SerializeField] bool _s3;


    virtual protected void OnValidate() 
    {
        boxcollider = GetComponent<BoxCollider2D>();
        root = transform.FindDeepChild("_ROOT_");
    }

    private bool _isopened = false;
    private void Awake()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        if (boxcollider == null)
            Debug.LogError("CUT ERROR ] BoxCollider2D 없음");

        root = transform.FindDeepChild("_ROOT_");
        if (root == null)
            Debug.LogError("CUT ERROR ] _ROOT_ 없음");

        List<SpriteRenderer> sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        List<TextMeshPro> texts = GetComponentsInChildren<TextMeshPro>().ToList();

        isfocused = false;

        OnAwake();

        pattern?.Set(root,sprites,texts);
    }

    

    protected virtual void OnEnable()
    {
        EpisodeEvent.I.eventCutFocused += OneventCutFocused;

        Open();   
    }

    protected virtual void OnDisable() 
    {
        EpisodeEvent.I.eventCutFocused -= OneventCutFocused;
    }

    protected virtual void Update()
    {
        if (isfocused == false) return;
            
        ability?.Tick();
    }



    public void Open(object obj = null)
    {
        if (_isopened == true) return;
        _isopened = true;

        ability?.Init();
        pattern?.Open();

        OnOpened(obj);      

        AdjustColliderSize();
    }

    public void Close()
    {
        if (_isopened == false) return;
        _isopened = false;
        
        OnClosed();

        if (pattern != null)
            pattern.Close();
        else
            gameObject.SetActive(false);
    }


    protected virtual void OnAwake() {}
    protected virtual void OnOpened(object obj = null) {}
    protected virtual void OnClosed() {}

    protected virtual void OneventCutFocused(EpisodeCamera epcam, Cut focusedcut)
    {
        if (epcam == null || focusedcut.name != this.name)
        {
            isfocused = false;
            return;
        }

        isfocused = true;
        ability?.Execute(epcam, this);
    }


    //
    // 충돌체 관련
    //
    private void AdjustColliderSize()
    {
        List<SpriteRenderer> sprites = GetComponentsInChildren<SpriteRenderer>().ToList();

        if (sprites.Count == 0)
        {
            Debug.LogWarning("SpriteRenderer를 찾을 수 없습니다.");
            return;
        }

        // 모든 스프라이트를 포함하는 Bounds 계산
        Bounds bounds = sprites[0].bounds;
        for (int i = 1; i < sprites.Count; i++)
        {
            bounds.Encapsulate(sprites[i].bounds);
        }

        // BoxCollider2D 크기 및 위치 설정
        boxcollider.size = new Vector3( bounds.size.x, bounds.size.y + extraHeight, bounds.size.z );
        boxcollider.offset = transform.InverseTransformPoint(bounds.center);

        root.localPosition = new Vector3(extraOffset.x, extraOffset.y, 0f);
    }


    public void TriggerSFX(AudioClip clip)
    {
        EpisodeEvent.I.TriggerSFXPlay(clip);
    }

}
