using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootSequence : MonoBehaviour
{
   [SerializeField] private Image blackScreen;
   [SerializeField] private AudioSource bootSound;

   private void Start()
   {
      StartCoroutine(BootSequenceCoroutine());
   }
   
   private IEnumerator BootSequenceCoroutine()
   {
      yield return new WaitForSeconds(3f);
      
      if (blackScreen != null)
      {
         blackScreen.gameObject.SetActive(false);
      }
      
      if (bootSound != null && bootSound.clip != null)
      {
         bootSound.Play();
         
         yield return new WaitForSeconds(bootSound.clip.length);
      }
      
      JumptoRulesScene();
   }
   
   private void JumptoRulesScene()
   {
      SceneManager.LoadScene("RulesScene", LoadSceneMode.Single);
   }
   
}
