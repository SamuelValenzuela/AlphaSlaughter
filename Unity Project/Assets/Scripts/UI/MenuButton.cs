using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>MenuButton</c> provides functions accessible through the pause menu
/// </summary>
public class MenuButton : MonoBehaviour {

    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource asource;

    private void Awake () {
        asource = GetComponent<AudioSource>();
        asource.ignoreListenerPause = true;
	}

    /// <summary>
    /// This method plays a sound when hovering over a button
    /// </summary>
    public void HoverSound()
    {
        asource.PlayOneShot(hoverSound);
    }

    /// <summary>
    /// This method plays a sound when clicking a button
    /// </summary>
    public void ClickSound()
    {
        asource.PlayOneShot(clickSound);
    }
}
