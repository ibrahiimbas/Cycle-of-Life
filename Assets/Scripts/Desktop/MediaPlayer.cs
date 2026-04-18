using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Video;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;
using UnityEngine.EventSystems;

public class MediaPlayer : MonoBehaviour
{
   [SerializeField] private AudioSource windowsSong;
   [SerializeField] private Toggle songToggle;
   [SerializeField] private Button stopSongButton;
   [SerializeField] private GameObject playIcon;
   [SerializeField] private Slider volumeSlider;

   [Header("Video")] 
   [SerializeField] private string videoFileName;
   [SerializeField] private VideoPlayer videoPlayer;
   [SerializeField] private GameObject videoScreen;
   
   [Header("Song Progress")]
   [SerializeField] private Slider progressSlider;
   [SerializeField] private TextMeshProUGUI timeText;
   [SerializeField]  private bool isDraggingSlider = false;
   
   private bool isVideoPlaying = false;

   private void Start()
   {
      videoScreen.SetActive(false);
      songToggle.onValueChanged.AddListener(PlaySong);
      stopSongButton.onClick.AddListener(StopSong);
      
      if (volumeSlider != null)
      {
         volumeSlider.minValue = 0f;
         volumeSlider.maxValue = 1f;
         volumeSlider.value = windowsSong.volume;
         volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
      }
      
      if (progressSlider != null)
      {
         progressSlider.minValue = 0f;
         progressSlider.maxValue = windowsSong.clip.length;
         progressSlider.value = 0f;
         
         progressSlider.onValueChanged.AddListener(OnProgressSliderChanged);
      }
      
      if (timeText != null && windowsSong.clip != null)
      {
         timeText.text = FormatTime(0f) + " / " + FormatTime(windowsSong.clip.length);
      }
      
      EventTrigger trigger = progressSlider.gameObject.AddComponent<EventTrigger>();

      var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
      pointerDown.callback.AddListener((_) => SetDragging(true));
      trigger.triggers.Add(pointerDown);

      var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
      pointerUp.callback.AddListener((_) => SetDragging(false));
      trigger.triggers.Add(pointerUp);
   }
   
   private void Update()
   {
      if (windowsSong != null && windowsSong.isPlaying && !isDraggingSlider && progressSlider != null)
      {
         progressSlider.value = windowsSong.time;
         
         if (timeText != null)
         {
            timeText.text = FormatTime(windowsSong.time) + " / " + FormatTime(windowsSong.clip.length);
         }
      }
      
      if (songToggle.isOn && windowsSong != null && !windowsSong.isPlaying)
      {
         StopSong();
      }
   }
   
   private void OnProgressSliderChanged(float value)
   {
      if (isDraggingSlider)
      {
         windowsSong.time = value;
         
         if (videoPlayer != null && videoPlayer.isPlaying)
         {
            videoPlayer.time = value;
         }
         
         if (timeText != null)
         {
            timeText.text = FormatTime(value) + " / " + FormatTime(windowsSong.clip.length);
         }
      }
   }
   
   public void SetDragging(bool dragging)
   {
      isDraggingSlider = dragging;

      if (!dragging)
      {
         windowsSong.time = progressSlider.value;

         if (videoPlayer != null)
            videoPlayer.time = progressSlider.value;
      }
   }
   
   private string FormatTime(float timeInSeconds)
   {
      int minutes = Mathf.FloorToInt(timeInSeconds / 60);
      int seconds = Mathf.FloorToInt(timeInSeconds % 60);
      return string.Format("{0:00}:{1:00}", minutes, seconds);
   }

   private void PlaySong(bool isOn)
   {
      if (isOn)
      {
         windowsSong.Play();
         playIcon.SetActive(false);
         // Play visuals
         PlayVideo();
      }
      else
      {
         windowsSong.Pause();
         playIcon.SetActive(true);
         
         // Pause video
         PauseVideo();
      }
   }

   private void StopSong()
   {
      windowsSong.Stop();
      songToggle.isOn = false;
      playIcon.SetActive(true);
      
      if (progressSlider != null)
      {
         progressSlider.value = 0f;
         if (timeText != null)
            timeText.text = FormatTime(0f) + " / " + FormatTime(windowsSong.clip.length);
      }
      
      // Reset video
      StopVideo();
   }

   public void StopAndResetSong()
   {
      windowsSong.Stop();
      songToggle.isOn = false;
      playIcon.SetActive(true);
      
      if (progressSlider != null)
      {
         progressSlider.value = 0f;
         if (timeText != null)
            timeText.text = FormatTime(0f) + " / " + FormatTime(windowsSong.clip.length);
      }
      
      // Reset video
      StopVideo();
   }
   
   private void OnVolumeChanged(float value)
   {
      if (windowsSong != null)
         windowsSong.volume = value;
   }

   private void PlayVideo()
   {
      if (videoPlayer)
      {
         videoScreen.SetActive(true);
         if (!videoPlayer.isPrepared || videoPlayer.frame == 0)
         {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
         }
         else if (!videoPlayer.isPlaying)
         {
            videoPlayer.Play();
         }
      }
   }

   private void PauseVideo()
   {
      if (videoPlayer != null && videoPlayer.isPlaying)
      {
         videoPlayer.Pause();
      }
   }

   private void StopVideo()
   {
      if (videoPlayer)
      {
         videoPlayer.Stop();
         videoScreen.SetActive(false);
      }
   }
}
