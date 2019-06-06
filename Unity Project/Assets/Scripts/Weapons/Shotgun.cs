using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Shotgun</c> needs to be attached to the Shotgun object
/// </summary>
public class Shotgun : Weapon
{

    public GameObject shotgunBullet;
    private float shotgunLength;

    public int bulletCount; // minimum: 2
    public float angle;
    private float angleStep;

    new protected void Awake()
    {
        base.Awake();
        shotgunLength = GetComponent<SpriteRenderer>().size.y;
        angleStep = angle / (bulletCount - 1);
    }

    /// <summary>
    /// This method implements the shotgun's attack logic
    /// </summary>
    protected override void Attack()
    {
        float currentAngle = -angle / 2;
        for (int i = 0; i < bulletCount; i++, currentAngle += angleStep)
        {
            Instantiate(shotgunBullet, transform.position + transform.TransformDirection(new Vector3(0, shotgunLength)), Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, currentAngle)));
        }
    }
}
