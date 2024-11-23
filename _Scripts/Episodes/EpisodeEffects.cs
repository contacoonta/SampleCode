using UnityEngine;
using CustomInspector;
using MoreMountains.Feedbacks;

public class EpisodeEffects : MonoBehaviour
{

    [Space(20)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("Sound FX 관련")][HideField,SerializeField] bool _l0;
    [Space] [SerializeField, ReadOnly] MMF_Player player;

    [Space(20)]
    [SerializeField] Vector2 pitchMinMax = Vector2.one;
    [SerializeField] Vector2 volumeMinMax = Vector2.one;
    
    [Space(50)] [HideField,SerializeField] bool _s3;



    private void Awake()
    {
        player = GetComponentInChildren<MMF_Player>();
        if (player == null)
            Debug.LogError("MMF_Player 없음");
    }

    private void OnEnable() 
    {
        EpisodeEvent.I.eventSFXPlay += OneventSFXPlayed;        
    }

    private void OnDisable() 
    {
        EpisodeEvent.I.eventSFXPlay -= OneventSFXPlayed;
    }

    private void OneventSFXPlayed(AudioClip clip)
    {
        PlaySfx(clip);
    }


    public void PlaySfx(AudioClip clip)
    {
        var snd = player.GetFeedbackOfType<MMF_MMSoundManagerSound>();

        if (snd == null && clip == null)
        {
            Debug.LogWarning("MMF_MMSoundManagerSound - Error | AudioClip - None");
            return;
        }
        
        snd.RandomSfx = new AudioClip[] {clip};
        snd.MinVolume = volumeMinMax.x;
        snd.MaxVolume = volumeMinMax.y;
        snd.MinPitch = pitchMinMax.x;
        snd.MaxPitch = pitchMinMax.y;

        player.PlayFeedbacks();
    }

}