using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>AirstrikeJet</c> needs to be attached to the AirstrikeJet object which is instantiated once the attack has started
/// </summary>
public class AirstrikeJet : MonoBehaviour {

    public GameObject rocket;
    public GameObject explosion;
    public float speed;
    public float rocketFallTime;
    public float distance; // has to be same as in Airstrike

    private bool attacked = false;
    private Vector2 startPoint;
    private Vector3 dropPoint;

    public void SetDropPoint(Vector2 dropPoint)
    {
        this.dropPoint = dropPoint;
    }

    private void Awake()
    {
        startPoint = transform.position;
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    private void FixedUpdate ()
    {
		if(!attacked && Vector2.Distance(startPoint, transform.position) > distance) // make rocket fall at correct location
        {
            attacked = true;
            StartCoroutine(FallingRocket());
        }
        else if (Vector2.Distance(startPoint, transform.position) > 2 * distance)
        {
            Destroy(gameObject);
        }
	}

    /// <summary>
    /// This coroutine makes the rocket fall below the jet
    /// </summary>
    private IEnumerator FallingRocket()
    {
        Vector3 rocketPos = dropPoint + new Vector3(0, 0, 1);
        GameObject fallingRocket = Instantiate(rocket, rocketPos, new Quaternion());
        yield return new WaitForSeconds(rocketFallTime);
        Destroy(fallingRocket);
        Instantiate(explosion, rocketPos, new Quaternion());
    }
}
