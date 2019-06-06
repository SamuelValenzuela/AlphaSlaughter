using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Pistol</c> needs to be attached to the Pistol object
/// </summary>
public class Pistol : Weapon {

    public GameObject pistolBullet;
    private float pistolLength;

    new protected void Awake()
    {
        base.Awake();
        pistolLength = GetComponent<SpriteRenderer>().size.y;
    }

    /// <summary>
    /// This method implements the pistol's attack logic
    /// </summary>
    protected override void Attack()
    {
        Instantiate(pistolBullet, transform.position + transform.TransformDirection(new Vector3(0, pistolLength)), transform.rotation);
    }
}
