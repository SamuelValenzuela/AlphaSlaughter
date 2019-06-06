using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>KillParticleSystem</c> destroys the particle system its attached to automatically when unused
/// </summary>
public class KillParticleSystem : MonoBehaviour
{
    private void Update()
    {
        if (!this.gameObject.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
