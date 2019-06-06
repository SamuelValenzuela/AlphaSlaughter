using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>GameMusic</c> needs to be attached to the game music to make it unaffected when the time is paused
/// </summary>
public class GameMusic : MonoBehaviour
{

    private void Awake () {
        GetComponent<AudioSource>().ignoreListenerPause = true;
	}
}
