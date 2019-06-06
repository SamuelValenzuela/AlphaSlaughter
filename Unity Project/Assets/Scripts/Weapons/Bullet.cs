using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Bullet</c> needs to be attached to the Bullet object which can be used and modified for any weapon
/// </summary>
public class Bullet : MonoBehaviour {

    public int damage;
    public float speed;
    public float timeToLive;
    public bool destroyOnHit;

    private float timeSinceSpawn = 0;

    private void FixedUpdate()
    {
        timeSinceSpawn += Time.fixedDeltaTime;
        if (timeSinceSpawn > timeToLive)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector2.up * speed, Space.Self);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(damage);
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
