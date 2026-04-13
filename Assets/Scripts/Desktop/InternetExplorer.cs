using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InternetExplorer : MonoBehaviour
{
   [Header("Time")] 
   [SerializeField] private TextMeshProUGUI time_text_1;
   [SerializeField] private TextMeshProUGUI time_text_2;
   [SerializeField] private TextMeshProUGUI time_text_3;
   [SerializeField] private TextMeshProUGUI time_text_4;
   
   [Header("Buttons")] 
   [SerializeField] private Button guestBookButton;
   [SerializeField] private Button homePageButton_1;
   [SerializeField] private Button homePageButton_2;
   [SerializeField] private Button homePageButton_3;
   [SerializeField] private Button aboutPageButton;
   [SerializeField] private Button mapPageButton;
   [SerializeField] private Button mailLinkButton;
   [SerializeField] private Button githubLinkButton;
   [SerializeField] private Button linkedinLinkButton;
   [SerializeField] private Button itchIOLinkButton;
   
   [Header("Pages and Scroll Rects")] 
   [SerializeField] private GameObject mainPage;
   [SerializeField] private GameObject guestPage;
   [SerializeField] private GameObject aboutPage;
   [SerializeField] private GameObject mapPage;
   [SerializeField] private ScrollRect mainPageScrollRect;
   [SerializeField] private ScrollRect guestPageScrollRect;
   [SerializeField] private ScrollRect aboutPageScrollRect;
   [SerializeField] private ScrollRect mapPageScrollRect;
   [SerializeField] private GameObject loadingPage;
   
   [Header("Page Loading Status")]
   [SerializeField] private TextMeshProUGUI pageStatusText;
   
   [Header("URL")] 
   [SerializeField] private string mailUrl = "mailto:ibrahimbas1414@gmail.com";
   [SerializeField] private string githubUrl = "https://github.com/ibrahiimbas";
   [SerializeField] private string linkedinUrl = "https://www.linkedin.com/in/ibrahimbas15/";
   [SerializeField] private string itchIoUrl = "https://synthseizer.itch.io/";

   [Header("Up Panel URL")]
   [SerializeField] private TextMeshProUGUI urlText;
   [SerializeField] private string mainPageUrl = "http://www.synthesizertr.com";
   [SerializeField] private string aboutMePageUrl = "http://www.synthesizertr.com/aboutme";
   [SerializeField] private string guestbookPagePageUrl = "http://www.synthesizertr.com/guestbook";
   [SerializeField] private string mapPagePageUrl = "http://www.synthesizertr.com/map";
   
   private string formattedTime;

   private void Start()
   {
      SetURL(mainPageUrl);
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
      mapPage.SetActive(false);
      loadingPage.SetActive(false);
      guestBookButton.onClick.AddListener(OnGuestBookClick);
      homePageButton_1.onClick.AddListener(OnHomePageButtonClick);
      homePageButton_2.onClick.AddListener(OnHomePageButtonClick);
      homePageButton_3.onClick.AddListener(OnHomePageButtonClick);
      aboutPageButton.onClick.AddListener(OnAboutPageButtonClick);
      mapPageButton.onClick.AddListener(OnMapPageButtonClick);
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
      time_text_4.text = formattedTime;
   }

   private void OnGuestBookClick()
   {
      SetURL(guestbookPagePageUrl);
      mainPage.SetActive(false);
      guestPage.SetActive(true);
      aboutPage.SetActive(false);
      mapPage.SetActive(false);
      StartCoroutine(LoadingSession());
      StartCoroutine(ResetScrollRectNextFrame(mainPageScrollRect));
   }

   private void OnHomePageButtonClick()
   {
      SetURL(mainPageUrl);
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
      mapPage.SetActive(false);
      StartCoroutine(LoadingSession());
      ResetMainScrollRect(false);
   }

   private void OnAboutPageButtonClick()
   {
      SetURL(aboutMePageUrl);
      mainPage.SetActive(false);
      guestPage.SetActive(false);
      aboutPage.SetActive(true);
      mapPage.SetActive(false);
      StartCoroutine(LoadingSession());
      ResetMainScrollRect(false);
   }

   private void OnMapPageButtonClick()
   {
      SetURL(mapPagePageUrl);
      mainPage.SetActive(false);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
      mapPage.SetActive(true);
      StartCoroutine(LoadingSession());
      ResetMainScrollRect(false);
   }
   
   private IEnumerator ResetScrollRectNextFrame(ScrollRect scrollRect)
   {
    
      yield return new WaitForEndOfFrame();

      if (scrollRect.vertical)
      {
         scrollRect.verticalNormalizedPosition = 1f;
      }
   }
   
   public void ResetMainScrollRect(bool resetUrl)
   {
      StartCoroutine(ResetScrollRectNextFrame(mainPageScrollRect));
      StartCoroutine(ResetScrollRectNextFrame(guestPageScrollRect));
      StartCoroutine(ResetScrollRectNextFrame(aboutPageScrollRect));
      StartCoroutine(ResetScrollRectNextFrame(mapPageScrollRect));
      
      if (resetUrl)
      {
         SetURL(mainPageUrl);
      }
   }

   private IEnumerator LoadingSession()
   {
      pageStatusText.text = "Opening page...";
      loadingPage.SetActive(true);
      yield return new WaitForSeconds(.5f);
      pageStatusText.text = "Done";
      loadingPage.SetActive(false);
   }

   public void ResetToHomePage()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      aboutPage.SetActive(false);
      mapPage.SetActive(false);
   }

   private void SetURL(string url)
   {
      urlText.text = url;
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
