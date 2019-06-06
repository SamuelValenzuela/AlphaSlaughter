using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class <c>Menu</c> provides functions accessible through the pause menu
/// </summary>
public class Menu : MonoBehaviour {

    public bool selectCursor;
    public Texture2D cursor;
    public Vector2 cursorHotspot;

    private void Awake()
    {
        if(selectCursor)
            Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }

    /// <summary>
    /// This method (re)loads a scene 
    /// </summary>
    public void LoadScene(string scene)
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// This method closes the game window
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

}