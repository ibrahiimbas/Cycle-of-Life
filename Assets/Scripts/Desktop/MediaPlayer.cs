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
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle; 

public class MediaPlayer : MonoBehaviour
{
   [SerializeField] private AudioSource windowsSong;
   [SerializeField] private Toggle songToggle;

   private void Start()
   {
      songToggle.onValueChanged.AddListener(PlaySong);
   }

   private void PlaySong(bool isOn)
   {
      if (isOn)
      {
         windowsSong.Play();
         // Play visuals
      }
      else
      {
         windowsSong.Pause();
      }
   }

   public void StopAndResetSong()
   {
      windowsSong.Stop();
      songToggle.isOn = false;
   }
}
