using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera; 
    public Slider speedSlider;
    public Slider zoomSlider;
    
    [SerializeField] private TextMeshProUGUI cameraMovementSpeedTxt;

    void Start()
    {
        speedSlider.onValueChanged.AddListener(OnSpeedSliderValueChanged);
        zoomSlider.onValueChanged.AddListener(OmZoomSliderValueChanged);
    }
    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalMove, verticalMove, 0f).normalized;
        transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
        cameraMovementSpeedTxt.text = math.round(moveSpeed).ToString();
    }
    
    void OnSpeedSliderValueChanged(float value)
    {
        if (mainCamera != null)
        {
            mainCamera.GetComponent<CameraScript>().moveSpeed = value;
        }
    }

    void OmZoomSliderValueChanged(float value)
    {
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = value;
        }
    }
    
}
