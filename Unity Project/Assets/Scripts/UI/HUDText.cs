using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>HUDText</c> needs to be attached to the HUDText object and keeps it up to date
/// </summary>
public class HUDText : MonoBehaviour {

    private Text text;

	private void Awake () {
        text = GetComponent<Text>();
	}

    private void Update () {
        text.text = "Score: " + Player.player.score + "\nAmmo: " + (Weapon.weapon.usesAmmo ? Weapon.Ammo + "\nCost: " + Weapon.weapon.ammoGain + "/" + Weapon.weapon.ammoCost : "∞");
	}
}
