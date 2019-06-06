using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class <c>Player</c> needs to be attached to the Player object and handles input and game logic affecting the player
/// </summary>
public class Player : MonoBehaviour
{

    public static Player player;

    [System.Serializable]
    public class InputSettings
    {
        public readonly string SIDEWAYS_AXIS = "Horizontal";
        public readonly string FORWARD_AXIS = "Vertical";
        public readonly string ATTACK_BUTTON = "Attack";
        public readonly string BUY_AMMO = "Buy Ammo";
        public readonly string SWITCH_WEAPON_AXIS = "Switch Weapon";
        public readonly string PAUSE = "Pause";
        public readonly string SELECT_SWORD = "Select Sword";
        public readonly string SELECT_PISTOL = "Select Pistol";
        public readonly string SELECT_SHOTGUN = "Select Shotgun";
        public readonly string SELECT_AIRSTRIKE = "Select Airstrike";
    }

    public int maxHealth;
    public float speed;
    public GameObject[] weapons;
    public float weaponHeight;
    public GameObject cam;
    public Color damagedColor;
    public float damageDuration;
    public Vector2[] spawnPositions;
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject controlMenu;

    public Texture2D cursor;
    public Vector2 cursorHotspot;
    public Texture2D crosshair;
    public Vector2 crosshairHotspot;
    public AudioClip[] damagedSounds;
    public AudioClip switchWeaponSound;
    public AudioClip buyAmmoSound;
    public AudioClip noMoneySound;


    private Animator animator;
    private Rigidbody2D rbody;
    private AudioSource asourcePlayer;
    private AudioSource asourceWeapon;

    [HideInInspector]
    public InputSettings inputSettings;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int score = 0;

    private float forwardInput, sidewaysInput;
    private Vector2 velocity;

    private GameObject currentWeaponObject;

    private bool frozen = false;

    private bool paused = false;

    private SpriteRenderer[] bodySprites;
    private Camera camComp;

    private void Awake()
    {
        player = this;
        health = maxHealth;

        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", false);
        rbody = GetComponent<Rigidbody2D>();
        asourcePlayer = GetComponents<AudioSource>()[0];
        asourceWeapon = GetComponents<AudioSource>()[1];
        bodySprites = GetComponentsInChildren<SpriteRenderer>();
        camComp = cam.GetComponent<Camera>();

        Cursor.SetCursor(crosshair, crosshairHotspot, CursorMode.Auto);

        transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)];
        velocity = Vector2.zero;

        Weapon.weaponAmmo = new int[weapons.Length];
        SelectWeapon(0);
    }

    private Vector3 camDiff = new Vector3(0, 0, -10); // camera's relative position to the player

    private void Update()
    {
        cam.transform.position = transform.position + camDiff;
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// This method calls other methods according to the player's input
    /// </summary>
    private void GetInput()
    {
        if (Input.GetButtonDown(inputSettings.PAUSE))
            TogglePause();
        if (!paused)
        {
            if (inputSettings.FORWARD_AXIS.Length != 0)
                forwardInput = Input.GetAxisRaw(inputSettings.FORWARD_AXIS);
            if (inputSettings.SIDEWAYS_AXIS.Length != 0)
                sidewaysInput = Input.GetAxisRaw(inputSettings.SIDEWAYS_AXIS);
            if (Input.GetButtonDown(inputSettings.ATTACK_BUTTON))
                Weapon.weapon.TryAttack();
            if (Input.GetAxisRaw(inputSettings.SWITCH_WEAPON_AXIS) < 0)
                SwitchWeapon(false);
            if (Input.GetAxisRaw(inputSettings.SWITCH_WEAPON_AXIS) > 0)
                SwitchWeapon(true);
            if (Input.GetButtonDown(inputSettings.BUY_AMMO))
                BuyAmmo();
            if (Input.GetButtonDown(inputSettings.SELECT_SWORD))
                SelectWeapon(0);
            if (Input.GetButtonDown(inputSettings.SELECT_PISTOL))
                SelectWeapon(1);
            if (Input.GetButtonDown(inputSettings.SELECT_SHOTGUN))
                SelectWeapon(2);
            if (Input.GetButtonDown(inputSettings.SELECT_AIRSTRIKE))
                SelectWeapon(3);
        }
    }

    /// <summary>
    /// This method controls the player's rotation and velocity
    /// </summary>
    private void Move()
    {
        if (frozen)
        {
            rbody.velocity = new Vector2(0, 0);
        }
        else
        {
            // look at mouse pointer
            Vector3 diff = camComp.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90, Vector3.forward);

            velocity.y = forwardInput;
            velocity.x = sidewaysInput;
            velocity.Normalize();
            rbody.velocity = velocity * speed;
            animator.SetBool("IsMoving", velocity.x != 0 || velocity.y != 0);
        }
    }

    /// <summary>
    /// This method toggles whether the game is paused or not
    /// </summary>
    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        AudioListener.pause = paused;
        pauseMenu.SetActive(paused);
        Cursor.SetCursor(paused ? cursor : crosshair, paused ? cursorHotspot : crosshairHotspot, CursorMode.Auto);
        if (!paused)
        {
            mainMenu.SetActive(true);
            controlMenu.SetActive(false);
        }
    }

    /// <summary>
    /// This method switches the selected weapon spot up or down
    /// </summary>
    public void SwitchWeapon(bool indexUp)
    {
        if (Weapon.weapon == null || (!Weapon.weapon.attacking && !Weapon.asource.isPlaying))
        {
            if (indexUp)
                SelectWeapon((Weapon.weaponIndex - 1 + weapons.Length) % weapons.Length);
            else
                SelectWeapon((Weapon.weaponIndex + 1) % weapons.Length);
        }
    }

    /// <summary>
    /// This method selects a weapon at a specific index
    /// </summary>
    private void SelectWeapon(int index)
    {
        if (index != Weapon.weaponIndex)
        {
            asourceWeapon.PlayOneShot(switchWeaponSound);
        }
        Weapon.weaponIndex = index;
        Destroy(currentWeaponObject);
        currentWeaponObject = Instantiate(weapons[index], transform.position + transform.rotation * new Vector3(0, 2.2f, weaponHeight), transform.rotation, transform);
        Weapon.weapon = currentWeaponObject.GetComponent<Weapon>();
    }

    /// <summary>
    /// This method tries to buy ammo for the currently selected weapon
    /// </summary>
    private void BuyAmmo()
    {
        if (Weapon.weapon.usesAmmo && score < Weapon.weapon.ammoCost)
        {
            asourceWeapon.PlayOneShot(noMoneySound);
        }
        if (score >= Weapon.weapon.ammoCost && Weapon.weapon.usesAmmo)
        {
            asourceWeapon.PlayOneShot(buyAmmoSound);
            score -= Weapon.weapon.ammoCost;
            Weapon.Ammo += Weapon.weapon.ammoGain;
        }
    }

    private int takingDamage = 0; // counter for the amount of current attacks being processed

    /// <summary>
    /// This method starts the damage coroutine
    /// </summary>
    public void Damage(int dmg)
    {
        StartCoroutine(DamageCoroutine(dmg));
    }

    /// <summary>
    /// This coroutine handles the player taking damage
    /// </summary>
    private IEnumerator DamageCoroutine(int dmg)
    {
        takingDamage++;
        health -= dmg;
        if (health <= 0) // check if the player's dead
        {
            Die();
        }
        else if (!asourcePlayer.isPlaying) // check if sound has to be played
        {
            asourcePlayer.clip = damagedSounds[Random.Range(0, damagedSounds.Length)];
            asourcePlayer.Play();
        }

        // let player's sprite blink in its damaged color shortly
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
    /// This method handles the player dying
    /// </summary>
    public void Die()
    {
        ScoreText.score = score;
        SceneManager.LoadScene("EndMenu");
    }

    /// <summary>
    /// This coroutine freezes the player's position and rotation for a specific amount of time
    /// </summary>
    public IEnumerator Freeze(float freezeDuration)
    {
        frozen = true;
        yield return new WaitForSeconds(freezeDuration);
        frozen = false;
    }
}
