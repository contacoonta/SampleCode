using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using MoreMountains.Feedbacks;

public class MMSoundControl : MonoBehaviour
{
    [Space(20)]
    [SerializeField, ReadOnly] MMF_Player player;

    [Space(20)]
    [SerializeField] Vector2 pitchMinMax = Vector2.one;
    [SerializeField] Vector2 volumeMinMax = Vector2.one;
    [SerializeField] AudioClip[] audioClips;
    

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
        if (player == null)
            Debug.LogError("MMF_Player 없음");
    }

    private void Start()
    {
        var snd = player.GetFeedbackOfType<MMF_MMSoundManagerSound>();

        if (snd != null && audioClips.Length > 0)
        {
            snd.RandomSfx = audioClips;
            snd.MinVolume = volumeMinMax.x;
            snd.MaxVolume = volumeMinMax.y;
            snd.MinPitch = pitchMinMax.x;
            snd.MaxPitch = pitchMinMax.y;

            player.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("MMF_MMSoundManagerSound 피드백을 찾을 수 없거나 오디오 클립이 없습니다.");
        }
    }
}