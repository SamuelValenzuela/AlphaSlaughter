using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>ReloadCircle</c> needs to be attached to the ReloadCircleFull object and keeps it up to date
/// </summary>
public class ReloadCircle : MonoBehaviour {

    private Image emptyCircle;

    private void Start () {
        emptyCircle = GetComponentsInChildren<Image>()[1];
	}

    private void Update () {
        if (Weapon.TimeSinceLastAttack > Weapon.weapon.timeBetweenAttacks)
        {
            emptyCircle.fillAmount = 1;
        }
        else
        {
            emptyCircle.enabled = true;
            emptyCircle.fillAmount = Weapon.TimeSinceLastAttack / Weapon.weapon.timeBetweenAttacks;
        }
    }
}
