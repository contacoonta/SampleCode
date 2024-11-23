
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using CustomInspector;
using TMPro;
using DG.Tweening;

public class InteractionDragDrop : AInteraction
{

    [Space(30)] [HideField,SerializeField] bool _s11;    
    [HorizontalLine][HideField,SerializeField] bool _l11;

    private int scoreMax;
    private int scoreCur;

    [SerializeField] TextMeshPro tmscore;


    public UnityAction<GameObject, GameObject> eventDragNDrop;

    private void OnEnable()
    {
        scoreMax = GetComponentsInChildren<DropControl>().Count();
        tmscore.text = $"{scoreCur}<size=70%>/</size>{scoreMax}";
    }
    

    public void TriggerDropped(GameObject drag, GameObject drop)
    {
        eventDragNDrop?.Invoke(drag,drop);
                
        ++scoreCur;
        tmscore.text = $"{scoreCur}<size=70%>/</size>{scoreMax}";

        if (scoreCur >= scoreMax)
        {
            tmscore.text = "완료!";
            tmscore.transform.DOPunchScale(Vector3.right*0.25f, 0.5f);

            DOVirtual.DelayedCall(1f, () => EpisodeEvent.I.TriggerPuzzleComplete());
        }
    }

}
