using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Weapon</c> is the basis for all weapons
/// </summary>
public abstract class Weapon : MonoBehaviour {
    
    public float timeBetweenAttacks;
    public bool usesAmmo;

    [HideInInspector]
    public bool attacking = false;
    public int ammoCost;
    public int ammoGain;

    public AudioClip attackSound;
    public AudioClip noAmmoSound;

    public static AudioSource asource;

    private static float timeSinceLastAttack = 0;
    public static float TimeSinceLastAttack { get { return timeSinceLastAttack; } }
    public static int Ammo { get { return weaponAmmo[weaponIndex]; } set { weaponAmmo[weaponIndex] = value; } }
    public static Weapon weapon;
    public static int[] weaponAmmo;
    public static int weaponIndex;

    protected void Awake () // make Awake() protected here so that child classes can override and call it
    {
        weapon = this;
        asource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;
    }

    /// <summary>
    /// This method performs an attack with the current weapon if possible
    /// </summary>
    public void TryAttack()
    {
        if(usesAmmo && Ammo <= 0)
        {
            asource.PlayOneShot(noAmmoSound);
        }
        else if(timeSinceLastAttack >= timeBetweenAttacks && (!usesAmmo || Ammo > 0))
        {
            if (usesAmmo)
                Ammo--;
            asource.clip = attackSound;
            asource.Play();
            Attack();
            timeSinceLastAttack = 0;
        }
    }

    /// <summary>
    /// This method needs to be implemented by the child classes to implement their individual attack logic
    /// </summary>
    protected abstract void Attack();

}
