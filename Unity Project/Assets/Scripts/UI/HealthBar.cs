using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>HealthBar</c> needs to be attached to the health bar object and keeps it up to date
/// </summary>
public class HealthBar : MonoBehaviour {

    private Image image;
    
	private void Start () {
        image = GetComponent<Image>();
	}
	
	private void Update () {
        image.fillAmount = (1/32f) + (15/16f) * (Player.player.health / (float) Player.player.maxHealth);
	}
}
