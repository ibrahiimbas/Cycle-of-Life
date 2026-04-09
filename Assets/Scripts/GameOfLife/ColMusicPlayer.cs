using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Track
{
    public string trackName;
    public AudioClip audioClip;
}

public class ColMusicPlayer : MonoBehaviour
{
    [Header("Music Player Buttons")] 
    [SerializeField] private Button nextTrackButton;
    [SerializeField] private Button previousTrackButton;
    [SerializeField] private Toggle playStopToggle;

    [Header("Track Text")] 
    [SerializeField] private TextMeshProUGUI trackName;
    
    [Header("Tracks")]
    [SerializeField] private List<Track> tracks = new List<Track>();
    
    
    [Header("Marquee Settings")]
    [SerializeField] private float marqueeSpeed = 60f;
    [SerializeField] private float marqueeDelayStart = 1.5f;
    [SerializeField] private float marqueeDelayLoop = 1f;
    [SerializeField] private float marqueeGap = 80f;
    
    [SerializeField] private AudioSource audioSource;
    private int currentTrackIndex = 0;
    private Coroutine marqueeCoroutine;
    
    private RectTransform maskRect;
    private RectTransform textRect;
    
    private void Awake()
    {
        if (trackName != null)
        {
            textRect = trackName.GetComponent<RectTransform>();
            maskRect = trackName.transform.parent.GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        nextTrackButton.onClick.AddListener(NextTrack);
        previousTrackButton.onClick.AddListener(PreviousTrack);
        playStopToggle.onValueChanged.AddListener(OnToggleChanged);
        
        if (tracks.Count > 0)
        {
            LoadTrack(currentTrackIndex);
            playStopToggle.isOn = true;
            PlayCurrentTrack();
        }
        else
        {
            Debug.LogWarning("Track list is empty!!");
        }
    }
    
    private void LoadTrack(int index)
    {
        if (tracks == null || tracks.Count == 0) return;
 
        currentTrackIndex = index;
        Track track = tracks[currentTrackIndex];
 
        if (audioSource != null && track.audioClip != null)
            audioSource.clip = track.audioClip;
 
        UpdateTrackNameUI(track.trackName);
    }
    
    private void PlayCurrentTrack()
    {
        if (audioSource == null || audioSource.clip == null) return;
        audioSource.Play();
    }


    private void NextTrack()
    {
        int nextIndex = (currentTrackIndex + 1) % tracks.Count;
        LoadTrack(nextIndex);
 
        if (playStopToggle.isOn)
            PlayCurrentTrack();
    }
    
    private void PreviousTrack()
    {
        int prevIndex = (currentTrackIndex - 1 + tracks.Count) % tracks.Count;
        LoadTrack(prevIndex);
 
        if (playStopToggle.isOn)
            PlayCurrentTrack();
    }

    private void OnToggleChanged(bool isOn)
    {
        if (audioSource == null) return;
 
        if (isOn)
        {
            if (audioSource.time > 0f || audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
            else
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }
    
    private void UpdateTrackNameUI(string name)
    {
        if (trackName == null) return;
 
        trackName.text = name;
 
        if (marqueeCoroutine != null)
            StopCoroutine(marqueeCoroutine);
 
        StartCoroutine(StartMarqueeAfterLayout(name));
    }
    
    private IEnumerator StartMarqueeAfterLayout(string name)
    {
        yield return null;
        yield return null;
 
        if (maskRect == null || textRect == null) yield break;
 
        float maskWidth = maskRect.rect.width;
 
        trackName.ForceMeshUpdate();
        float textWidth = trackName.preferredWidth;
        
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth);
 
        if (textWidth <= maskWidth)
        {
            textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskWidth);
            textRect.anchoredPosition = new Vector2(0, textRect.anchoredPosition.y);
            yield break;
        }
 
        marqueeCoroutine = StartCoroutine(MarqueeLoop(textWidth, maskWidth));
    }
 
    private IEnumerator MarqueeLoop(float textWidth, float maskWidth)
    {
        float startX = 32f;
        float endX = -(textWidth + marqueeGap);
 
        textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
        yield return new WaitForSeconds(marqueeDelayStart);
 
        while (true)
        {
            float currentX = startX;
 
            while (currentX > endX)
            {
                currentX -= marqueeSpeed * Time.deltaTime;
                textRect.anchoredPosition = new Vector2(currentX, textRect.anchoredPosition.y);
                yield return null;
            }
 
            textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
 
            yield return new WaitForSeconds(marqueeDelayLoop);
        }
    }
 
    private void OnDestroy()
    {
        if (marqueeCoroutine != null)
            StopCoroutine(marqueeCoroutine);
    }
}
