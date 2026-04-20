using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlueScreen : MonoBehaviour
{
   [SerializeField] private GameObject blueScreen;
   [SerializeField] private AudioSource errorSound;
   [SerializeField] private MediaPlayer mediaPlayer;

   private void Start()
   {
      if (blueScreen != null)
      {
         blueScreen.SetActive(false);
      }
   }

   public void BlueScreenError()
   {
      StartCoroutine(ShutPCDownCoroutine());
   }
   
   private IEnumerator ShutPCDownCoroutine()
   {
      yield return new WaitForSeconds(2f);
      if (blueScreen != null)
      {
         Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;
         
         blueScreen.SetActive(true);
         errorSound.Play();
         
         mediaPlayer.StopAndResetSong();
         
         yield return new WaitForSeconds(5f);
         
         SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
      }
   }
}
