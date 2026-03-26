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

   void Start()
   {
      // Get the AudioSource component attached to this GameObject
      audioSource = GetComponent<AudioSource>();
      if (audioSource == null)
      {
         // Add an AudioSource if one doesn't exist
         audioSource = gameObject.AddComponent<AudioSource>();
      }
   }

   void Update()
   {
      // Check if the left mouse button is clicked
      if (Input.GetMouseButtonDown(0) && clickSound != null)
      {
         audioSource.PlayOneShot(clickSound);
      }
   }
}
