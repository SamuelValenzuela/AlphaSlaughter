using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Zombie</c> needs to be attached to the Zombie object and implements features only proper to zombies
/// </summary>
public class Zombie : Enemy {

    private bool headOff;

    new protected void Update()
    {
        base.Update();

        // remove the zombie's head after losing half of its health
        if (!headOff && health <= maxHealth / 2)
        {
            bodySprites[2].enabled = false;
            headOff = true;
        }
    }
}