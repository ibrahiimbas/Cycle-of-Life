using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskbarManager : MonoBehaviour
{
    [SerializeField] private List<TaskbarAppData> taskbarApps;
    private void Start()
    {
        
    }
}


[System.Serializable]
public class TaskbarAppData
{
    public string appName;
    public Toggle togglePrefab;
    public Sprite appIcon;
    public GameObject targetWindow;
}