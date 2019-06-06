using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>TextColorSwitcher</c> makes the text object it's attached to switch colors in specific time intervals
/// </summary>
public class TextColorSwitcher : MonoBehaviour
{
    public Color[] colors;
    public bool snake;
    public float switchTime;

    private Text text;
    private float timeSinceLastSwitch;
    private bool forward = true;
    private int index = 0;

    private void Awake()
    {
        text = GetComponent<Text>();
        text.color = colors[index];
    }

    private void Update()
    {
        timeSinceLastSwitch += Time.deltaTime;
        if (timeSinceLastSwitch >= switchTime)
        {
            SwitchColor();
            timeSinceLastSwitch = 0;
        }
    }

    /// <summary>
    /// This method switches to the next color
    /// </summary>
    private void SwitchColor() {
        if(colors.Length > 1)
        {
            NextIndex();
        }
        text.color = colors[index];
    }

    /// <summary>
    /// This method determines the next color index
    /// </summary>
    private void NextIndex()
    {
        index += forward ? 1 : -1;
        if (index == colors.Length)
        {
            if (snake)
            {
                index -= 2;
                forward = false;
            } else
            {
                index = 0;
            }
        } else if (index < 0) {
            index = 1;
            forward = true;
        }
    }
}