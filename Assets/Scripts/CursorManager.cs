using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorMain;
    [SerializeField] private Texture2D cursorClickable;
    [SerializeField] private Texture2D cursorLoading;

    private Vector2 cursorHotspot;

    private void Start()
    {
        cursorHotspot = new Vector2(cursorMain.width / 2, cursorMain.height / 2);
        Cursor.SetCursor(cursorMain,cursorHotspot,CursorMode.Auto);
    }
}
