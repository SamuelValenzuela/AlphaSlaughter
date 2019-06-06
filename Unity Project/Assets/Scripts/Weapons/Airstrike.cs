using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Airstrike</c> needs to be attached to the Airstrike object
/// </summary>
public class Airstrike : Weapon {

    public Sprite inactiveButton;
    public Sprite pressedButton;
    public GameObject jet;
    public float distance;

    /// <summary>
    /// This method implements the airstrike's attack logic
    /// </summary>
    protected override void Attack()
    {
        attacking = true;
        GetComponent<SpriteRenderer>().sprite = pressedButton;
        StartCoroutine(Attacking());

        // spawn jet
        Vector2 dropPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float deg = Random.value * 360;
        Vector2 spawnLocation = dropPoint + new Vector2(Mathf.Sin(deg * Mathf.Deg2Rad) * distance, Mathf.Cos(deg * Mathf.Deg2Rad) * distance);
        GameObject spawnedJet = Instantiate(jet, spawnLocation, Quaternion.Euler(0, 0, -deg + 180));
        spawnedJet.GetComponent<AirstrikeJet>().SetDropPoint(dropPoint);
    }

    /// <summary>
    /// This coroutine controls the airstrike button's appearance while pressing it
    /// </summary>
    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sprite = inactiveButton;
        attacking = false;
    }
}
