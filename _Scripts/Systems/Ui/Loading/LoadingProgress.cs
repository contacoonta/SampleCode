
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CustomInspector;


public class LoadingProgress : BehaviourSingleton<LoadingProgress>
{
    [Space(20)] [HideField] public bool _s0;
    [HorizontalLine("Progress Gauge 관련"),HideField] public bool _l0;

    [SerializeField] CanvasGroup progressCanvas;
    [SerializeField] Slider progressSlider;
    [SerializeField] Text progressText;



    [Space(20)] [HideField] public bool _s1;
    [HorizontalLine("Fade In-Out 관련"),HideField] public bool _l1;

    [SerializeField] CanvasGroup fadeCanvas;



    protected override bool IsDontdestroy() => true;
    public override void OnAwaked() {}


    private void OnEnable()
    {
        GlobalEvent.I.eventLoadScene += OneventLoadScene;
        GlobalEvent.I.eventLoadSceneComplete += OneventLoadSceneComplete;
    }

    private void OnDisable()
    {
        GlobalEvent.I.eventLoadScene -= OneventLoadScene;
        GlobalEvent.I.eventLoadSceneComplete -= OneventLoadSceneComplete;
    }

    private void OneventLoadScene(string scnname, bool showprogress)
    {
        if (showprogress)
        {
            Init();
            StartCoroutine(loadingprogress(scnname));
        }
        else
        {
            StartCoroutine(loadingwithoutprogress(scnname));
        }
    }

    private void OneventLoadSceneComplete()
    {
        Complete();
    }


    private IEnumerator loadingwithoutprogress(string scnname, bool additive = false)
    {
        fadeCanvas.gameObject.SetActive(true);
        yield return fadeCanvas.DOFade(1, 0.5f).SetEase(Ease.OutFlash).WaitForCompletion();

        AsyncOperation op = SceneManager.LoadSceneAsync(scnname, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;
        }

        yield return null;

        GlobalEvent.I.LoadComplete();
    }

    private IEnumerator loadingprogress(string scnname, bool additive = false)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scnname, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            // op.progress는 로딩 중에 0에서 0.9까지의 범위를 가짐
            float progress = Mathf.Clamp01(op.progress / 0.9f); 
            Progress(progress, ()=> op.allowSceneActivation = true);

            yield return null;
        }

        GlobalEvent.I.LoadComplete();        
    }



    public void Init()
    {
        progressCanvas.gameObject.SetActive(true);
        progressSlider.value = 0;
        progressText.text = "0";

        progressCanvas.alpha = 1;
    }

    public void Complete()
    {
        progressCanvas.alpha = 1;
        progressCanvas.DOFade(0f, 0.25f).OnComplete(()=> progressCanvas.gameObject.SetActive(false));
        
        fadeCanvas.alpha = 1;
        fadeCanvas.DOFade(0, 0.5f).SetEase(Ease.InFlash).OnComplete(()=> fadeCanvas.gameObject.SetActive(false));
    }    

    // value : 0f ~ 1f
    public void Progress(float value, UnityAction oncomplete = null)
    {
        float lerpSpeed = Application.isMobilePlatform ? 3f : 5f;
        
        progressSlider.value = Mathf.Lerp(progressSlider.value, value, lerpSpeed * Time.deltaTime);
        progressText.text = ((int)(progressSlider.value * 100f)).ToString();

        if (progressSlider.value >= 0.99f && value >= 0.9f)
        {
            progressSlider.value = 1f;
            oncomplete?.Invoke();
        }
    }



    public void FakeLoad(float duration = 2f)
    {
        progressCanvas.alpha = 1;
        DOVirtual.Float(progressSlider.value, 1f, duration, v => Progress(v))
            .OnComplete(()=>progressCanvas.DOFade(0f, 0.5f).OnComplete(()=> progressCanvas.gameObject.SetActive(false)));
    }

}
