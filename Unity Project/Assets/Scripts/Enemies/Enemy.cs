using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Enemy</c> is the basis for all enemies
/// </summary>
public class Enemy : MonoBehaviour
{

    public int health;
    public float velocity;
    public int value;
    public float attackSpeed;
    public int damage;
    public GameObject playerDamageParticles;
    public Color damagedColor;
    public float damageDuration;
    public GameObject dieParticles;
    public float minPassiveDelay;
    public float maxPassiveDelay;

    public float maxSoundDistance;
    public AudioClip[] passiveSounds;
    public AudioClip[] attackingSounds;
    public AudioClip[] damagedSounds;
    public AudioClip[] dyingSounds;

    private Rigidbody2D rbody;
    private AudioSource asource;
    private bool playingPassiveSound;
    private Transform playerTransform;
    protected SpriteRenderer[] bodySprites;
    protected int maxHealth;
    private float timeSinceLastAttack;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        asource = GetComponent<AudioSource>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        timeSinceLastAttack = attackSpeed;
        maxHealth = health;
        bodySprites = GetComponentsInChildren<SpriteRenderer>();
    }

    protected void Update() // make Update() protected here so that child classes can override and call it
    {
        timeSinceLastAttack += Time.deltaTime;
        // adjust volume according to distance to player
        asource.volume = (maxSoundDistance - Vector3.Distance(transform.position, Player.player.transform.position)) / maxSoundDistance;
        TryPassiveSound();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// This method schedules an enemy's passive sound for a random time in the future
    /// </summary>
    private void TryPassiveSound()
    {
        if (!asource.isPlaying && Time.timeScale != 0)
        {
            playingPassiveSound = true;
            asource.clip = passiveSounds[Random.Range(0, passiveSounds.Length)];
            asource.PlayDelayed(Random.Range(minPassiveDelay, maxPassiveDelay));
        }
    }

    /// <summary>
    /// This method defines the default enemy move function: look at and move towards the player
    /// </summary>
    private void Move()
    {
        Vector3 diff = playerTransform.position - transform.position;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        rbody.velocity = transform.up * velocity;
    }

    // private variables only relevant to damaging/dying methods
    private int takingDamage = 0;
    private bool dead = false;

    /// <summary>
    /// This method starts the damage coroutine
    /// </summary>
    public void Damage(int dmg)
    {
        StartCoroutine(DamageCoroutine(dmg));
    }

    /// <summary>
    /// This coroutine handles the enemy taking damage
    /// </summary>
    private IEnumerator DamageCoroutine(int dmg)
    {
        takingDamage++;
        health -= dmg;
        if (health <= 0) // check if the enemy's dead
        {
            if (!dead)
            {
                dead = true;
                Die();
            }
            yield break;
        }
        else if (!asource.isPlaying || playingPassiveSound) // check if sound has to be played
        {
            playingPassiveSound = false;
            asource.clip = damagedSounds[Random.Range(0, damagedSounds.Length)];
            asource.Play();
        }

        // let enemy's sprite blink in its damaged color shortly
        foreach (SpriteRenderer part in bodySprites)
        {
            part.color = damagedColor;
        }
        yield return new WaitForSeconds(damageDuration);
        if (--takingDamage == 0)
        {
            foreach (SpriteRenderer part in bodySprites)
            {
                part.color = Color.white;
            }
        }
    }
    
    /// <summary>
    /// This method handles the enemy dying
    /// </summary>
    private void Die()
    {
        Player.player.score += value;
        GameObject particleObject = Instantiate(dieParticles, transform.position, transform.rotation);
        particleObject.transform.localScale = transform.localScale;
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
        asource.clip = dyingSounds[Random.Range(0, dyingSounds.Length)];
        asource.Play();
        Destroy(gameObject, asource.clip.length);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Attack(col.collider);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        Attack(col.collider);
    }

    /// <summary>
    /// This method handles the enemy attacking the player if the conditions are met
    /// </summary>
    private void Attack(Collider2D collider)
    {
        if (collider.gameObject.name == "Player" && timeSinceLastAttack >= attackSpeed)
        {
            if (!asource.isPlaying || playingPassiveSound)
            {
                playingPassiveSound = false;
                asource.clip = attackingSounds[Random.Range(0, attackingSounds.Length)];
                asource.Play();
            }
            timeSinceLastAttack = 0;
            collider.GetComponent<Player>().Damage(damage);

            // spawn attack particles
            Vector2 pos = (transform.position - (1 - transform.localScale.x) * (collider.transform.position - transform.position) + collider.transform.position) * 0.5f;
            GameObject particles = Instantiate(playerDamageParticles, pos, playerDamageParticles.transform.rotation, collider.transform);
            ParticleSystem.ShapeModule sh = particles.GetComponent<ParticleSystem>().shape;
            sh.rotation = new Vector3(180, -transform.rotation.eulerAngles.z, 0);
        }
    }
}
