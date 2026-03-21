using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFirstLoad : MonoBehaviour
{
    private static HashSet<string> loadedScenes = new HashSet<string>();
    [SerializeField]private RulesSceneScript sceneScript;
    
    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (!loadedScenes.Contains(currentScene))
        {
            Debug.Log($"{currentScene} scene loaded first time");
            StartCoroutine(OnFirstTimeLoadCoroutine());
            
            loadedScenes.Add(currentScene);
        }
    }
    
    private IEnumerator OnFirstTimeLoadCoroutine()
    {
        yield return new WaitForSeconds(1f);
        sceneScript.FirstInfoPanelOpen();
    }
    
}