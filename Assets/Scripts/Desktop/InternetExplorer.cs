using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InternetExplorer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI time_text_1;
   [SerializeField] private TextMeshProUGUI time_text_2;
   [SerializeField] private TextMeshProUGUI time_text_3;
   [SerializeField] private Button guestBookButton;
   [SerializeField] private Button homePageButton_1;
   [SerializeField] private Button homePageButton_2;
   [SerializeField] private Button aboutPageButton;
   [SerializeField] private Button mailLinkButton;
   [SerializeField] private Button githubLinkButton;
   [SerializeField] private Button linkedinLinkButton;
   [SerializeField] private Button itchIOLinkButton;
   [SerializeField] private GameObject mainPage;
   [SerializeField] private GameObject guestPage;
   [SerializeField] private GameObject aboutPage;
   [SerializeField] private ScrollRect mainPageScrollRect;
   [SerializeField] private ScrollRect guestPageScrollRect;
   [SerializeField] private ScrollRect aboutPageScrollRect;
   
   [Header("URL")] 
   [SerializeField] private string mailUrl = "mailto:ibrahimbas1414@gmail.com";
   [SerializeField] private string githubUrl = "https://github.com/ibrahiimbas";
   [SerializeField] private string linkedinUrl = "https://www.linkedin.com/in/ibrahimbas15/";
   [SerializeField] private string itchIoUrl = "https://synthseizer.itch.io/";
   
   private string formattedTime;

   private void Start()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
      guestBookButton.onClick.AddListener(OnGuestBookClick);
      homePageButton_1.onClick.AddListener(OnHomePageButtonClick);
      homePageButton_2.onClick.AddListener(OnHomePageButtonClick);
      aboutPageButton.onClick.AddListener(OnAboutPageButtonClick);
      mailLinkButton.onClick.AddListener(OpenMailLink);
      githubLinkButton.onClick.AddListener(OpenGithubLink);
      linkedinLinkButton.onClick.AddListener(OpenLinkedinLink);
      itchIOLinkButton.onClick.AddListener(OpenitchIOLink);
   }

   private void Update()
   {
      DateTime currentTime = DateTime.Now;
      formattedTime = currentTime.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture) + "\n" + currentTime.ToString("dd MMMM", CultureInfo.InvariantCulture) + " 2000";
      time_text_1.text = formattedTime;
      time_text_2.text = formattedTime;
      time_text_3.text = formattedTime;
   }

   private void OnGuestBookClick()
   {
      mainPage.SetActive(false);
      guestPage.SetActive(true);
      aboutPage.SetActive(false);
      StartCoroutine(ResetScrollRectNextFrame(mainPageScrollRect));
   }

   private void OnHomePageButtonClick()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
      ResetMainScrollRect();
   }

   private void OnAboutPageButtonClick()
   {
      mainPage.SetActive(false);
      guestPage.SetActive(false);
      aboutPage.SetActive(true);
      ResetMainScrollRect();
   }
   
   private IEnumerator ResetScrollRectNextFrame(ScrollRect scrollRect)
   {
    
      yield return new WaitForEndOfFrame();

      if (scrollRect.vertical)
      {
         scrollRect.verticalNormalizedPosition = 1f;
      }
   }
   
   public void ResetMainScrollRect()
   {
      StartCoroutine(ResetScrollRectNextFrame(mainPageScrollRect));
      StartCoroutine(ResetScrollRectNextFrame(guestPageScrollRect));
      StartCoroutine(ResetScrollRectNextFrame(aboutPageScrollRect));
   }

   public void ResetToHomePage()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
   }
   
   private void OpenExternalLink(string url)
   {
      Application.OpenURL(url);
   }

   private void OpenMailLink()
   {
      OpenExternalLink(mailUrl);
   }

   private void OpenGithubLink()
   {
      OpenExternalLink(githubUrl);
   }

   private void OpenLinkedinLink()
   {
      OpenExternalLink(linkedinUrl);
   }

   private void OpenitchIOLink()
   {
      OpenExternalLink(itchIoUrl);
   }
}
