using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>AirstrikeExplosion</c> needs to be attached to the AirstrikeExplosion object which appears after the rocket has arrived
/// </summary>
public class AirstrikeExplosion : MonoBehaviour {

    public int damage;
    public float scaleStep;
    public float maxScale;
	
	private void FixedUpdate () {
		if(transform.localScale.x < maxScale)
        {
            transform.localScale += new Vector3(scaleStep, scaleStep);
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, GetComponent<AudioSource>().clip.length);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(damage);
        }
    }
}
