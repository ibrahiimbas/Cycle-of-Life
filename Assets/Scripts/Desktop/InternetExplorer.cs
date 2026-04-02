using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class InternetExplorer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI time_text_1;
   
   private string formattedTime;

   private void Update()
   {
      DateTime currentTime = DateTime.Now;
      formattedTime = currentTime.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture) + "\n" + currentTime.ToString("dd MMMM", CultureInfo.InvariantCulture) + " 2000";
      time_text_1.text = formattedTime;
   }
}
