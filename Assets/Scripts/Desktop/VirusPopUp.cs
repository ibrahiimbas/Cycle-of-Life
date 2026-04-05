using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class VirusPopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private Button closePopUpButton;
    [SerializeField] private Button mistakeButton;
    [SerializeField] private AudioSource congratulationsSound;

    private bool isActive = false;

    private void Start()
    {
        popUp.SetActive(false);
        isActive = false;

        closePopUpButton.onClick.AddListener(ClosePopUp);
        mistakeButton.onClick.AddListener(OnMistakeButtonClicked);
    }

    public void InitializePopUp()
    {
        if (!isActive)
        {
            popUp.SetActive(true);
            isActive = true;
            CheckSound();
        }
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
        isActive = false;
        CheckSound();
    }

    private void OnMistakeButtonClicked()
    {
        Debug.Log("Mistake button clicked.");
        popUp.SetActive(false);
        isActive = false;
        CheckSound();
    }

    private void CheckSound()
    {
        if (isActive && !congratulationsSound.isPlaying)
        {
            congratulationsSound.Play();
        }
        else if (!isActive && congratulationsSound.isPlaying)
        {
            congratulationsSound.Stop();
        }
    }
}