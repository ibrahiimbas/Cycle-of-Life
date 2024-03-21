using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class ClickSoundEffectScript : MonoBehaviour
{
   public AudioClip clickSound;
   private AudioSource audioSource;

   private void Start()
   {
      audioSource = GetComponent<AudioSource>();
      Button[] buttons = FindObjectsOfType<Button>();
      foreach (Button button in buttons)
      {
         button.onClick.AddListener(()=>PlayClickSound());
      }
   }

   void PlayClickSound()
   {
      audioSource.PlayOneShot(clickSound);
      UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null); //Do not focus the button after pressed
   }
}
