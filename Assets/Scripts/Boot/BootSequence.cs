using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BootSequence : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private AudioSource bootSound;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private GameObject bootImage;
    [SerializeField] private string videoFileName;
    [SerializeField] private VideoClip videoClip;
    
    private void Start()
    {
        Cursor.visible = false;
#if UNITY_WEBGL
        StartCoroutine(PlayForWebBuild());
#else
        StartCoroutine(BootSequenceCoroutine());
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            JumpToRulesScene();
        }
    }

#if !UNITY_WEBGL
    private IEnumerator BootSequenceCoroutine()
    {
        if (videoPlayer != null && videoDisplay != null)
        {
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.skipOnDrop = true;
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClip;
            videoPlayer.targetTexture = renderTexture;
            videoDisplay.texture = renderTexture;
            videoDisplay.gameObject.SetActive(false);
        }
        
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
            blackScreen.transform.SetAsFirstSibling();
        }
        
        yield return new WaitForSeconds(1f);
        
        if (videoDisplay != null)
        {
            videoDisplay.transform.SetSiblingIndex(blackScreen.transform.GetSiblingIndex() - 1);
        }
        
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            
            videoDisplay.gameObject.SetActive(true);
            
            float videoDuration = (float)videoPlayer.clip.length;
            float elapsedTime = 0f;
            
            while (elapsedTime < videoDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(false);
        }
        
        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(false);
        }
        
        if (bootSound != null && bootSound.clip != null)
        {
            bootImage.SetActive(true);
            bootSound.Play();
            yield return new WaitForSeconds(bootSound.clip.length);
        }
        
        JumpToRulesScene();
    }
#endif
    
    private void JumpToRulesScene()
    {
        SceneManager.LoadScene("RulesScene", LoadSceneMode.Single);
    }

#if UNITY_WEBGL
    private IEnumerator PlayForWebBuild()
    {
        if (videoPlayer != null && videoDisplay != null)
        {
            videoPlayer.waitForFirstFrame = false;
            videoPlayer.skipOnDrop = false;
            videoPlayer.source = VideoSource.Url;
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.targetTexture = renderTexture;
            videoDisplay.texture = renderTexture;
            videoDisplay.gameObject.SetActive(false);
        }

        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
            blackScreen.transform.SetAsFirstSibling();
        }

        yield return new WaitForSeconds(1f);

        if (videoDisplay != null)
        {
            videoDisplay.transform.SetSiblingIndex(blackScreen.transform.GetSiblingIndex() - 1);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoDisplay.gameObject.SetActive(true);

            yield return new WaitUntil(() => videoPlayer.isPrepared);

            float videoDuration = (float)videoPlayer.length;
            float elapsedTime = 0f;

            while (elapsedTime < videoDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        if (blackScreen != null)
            blackScreen.gameObject.SetActive(false);

        if (videoDisplay != null)
            videoDisplay.gameObject.SetActive(false);

        if (bootSound != null && bootSound.clip != null)
        {
            bootImage.SetActive(true);
            bootSound.Play();
            yield return new WaitForSeconds(bootSound.clip.length);
        }

        JumpToRulesScene();
    }
#endif
}