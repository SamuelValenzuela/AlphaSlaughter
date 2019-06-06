using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Sword</c> needs to be attached to the Sword object
/// </summary>
public class Sword : Weapon {

    public float attackDuration;
    public int damage;

    public Sprite inactiveSword;
    public Sprite attackingSword;
    
    private Collider2D coll;

    new protected void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
    }

    /// <summary>
    /// This method implements the airstrike's attack logic
    /// </summary>
    protected override void Attack()
    {
        attacking = true;
        GetComponent<SpriteRenderer>().sprite = attackingSword;
        coll.enabled = true;
        StartCoroutine(Player.player.Freeze(attackDuration));
        StartCoroutine(Attacking());
    }

    /// <summary>
    /// This coroutine controls the sword's appearance while attacking
    /// </summary>
    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackDuration);
        coll.enabled = false;
        GetComponent<SpriteRenderer>().sprite = inactiveSword;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(damage);
        }
    }

}
