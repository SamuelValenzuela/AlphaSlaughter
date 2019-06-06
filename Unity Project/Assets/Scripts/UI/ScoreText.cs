using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>SoreText</c> needs to be attached to the ScoreText object and makes it display the right score
/// </summary>
public class ScoreText : MonoBehaviour {

    public static int score;

    private void Awake ()
    {
            GetComponent<Text>().text = "You died!\nScore: " + score;
	}
}
