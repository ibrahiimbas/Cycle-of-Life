using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InternetExplorer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI time_text_1;
   [SerializeField] private TextMeshProUGUI time_text_2;
   [SerializeField] private Button guestBookButton;
   [SerializeField] private Button homePageButton_1;
   [SerializeField] private GameObject mainPage;
   [SerializeField] private GameObject guestPage;
   [SerializeField] private ScrollRect mainPageScrollRect;
   [SerializeField] private ScrollRect guestPageScrollRect;
   
   private string formattedTime;

   private void Start()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
      guestBookButton.onClick.AddListener(OnGuestBookClick);
      homePageButton_1.onClick.AddListener(OnHomePageButtonClick);
   }

   private void Update()
   {
      DateTime currentTime = DateTime.Now;
      formattedTime = currentTime.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture) + "\n" + currentTime.ToString("dd MMMM", CultureInfo.InvariantCulture) + " 2000";
      time_text_1.text = formattedTime;
      time_text_2.text = formattedTime;
   }

   private void OnGuestBookClick()
   {
      mainPage.SetActive(false);
      guestPage.SetActive(true);
      StartCoroutine(ResetScrollRectNextFrame(mainPageScrollRect));
   }

   private void OnHomePageButtonClick()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
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
   }

   public void ResetToHomePage()
   {
      mainPage.SetActive(true);
      guestPage.SetActive(false);
   }
}
