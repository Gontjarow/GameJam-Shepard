using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // "instrument" or "bow"
    public string Holding = "instrument";
    public AudioSource Sound;
    public GameObject Projectile;
    public bool PlayingMusic = false;
    public float Speed;
    public float ArrowSpeed;

    public List<Sprite> Sprites;
    public enum Show
    {
        fluteFront,
        fluteSide,
        bowFront,
        bowSide,
        bodyFront,
        bodySide
    }

    private Vector3 velocity;
    private SpriteRenderer body;
    private SpriteRenderer hand;

    private float tutorialTextTimer = 4;
	
    float dirX, dirY, dz;

    private void Start()
    {
        body = transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
        hand = transform.GetChild(2).GetComponentInChildren<SpriteRenderer>();
        Sound = transform.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
		dz = 0.1f;
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");
        velocity = (new Vector3(dirX, dirY, 0.0f).normalized) * Speed;
        transform.Translate(velocity * Time.fixedDeltaTime);
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * dz);

    }

    private void Update()
    {
        if(tutorialTextTimer != 0)
        {
            if((tutorialTextTimer -= Time.deltaTime) <= 0)
            {
                transform.GetChild(3).gameObject.SetActive(false);
                tutorialTextTimer = 0;
            }
        }

        // Body sprite control
        if(dirX != 0)
        {
            // Moving
            body.sprite = Sprites[(int)Show.bodySide];

            if(Holding == "bow")
                hand.sprite = Sprites[(int)Show.bowSide];
            else
                hand.sprite = Sprites[(int)Show.fluteSide];

            body.flipX = hand.flipX = (dirX < 0);
        }
        else 
        {
            body.sprite = Sprites[(int)Show.bodyFront];

            if(Holding == "bow")    hand.sprite = Sprites[(int)Show.bowFront];
            else                    hand.sprite = Sprites[(int)Show.fluteFront];

            body.flipX = hand.flipX = false;
            // TODO: Idk test??
        }

        // Use tool
        if(Holding == "instrument" && Input.GetKey(KeyCode.Mouse0))
        {
            //PlayingMusic = true;
            Sound.mute = false;
        }

        else if(Holding == "bow" && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - (Vector2)this.transform.position).normalized;
            float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

            GameObject arrow = Instantiate(Projectile,
                this.transform.position + new Vector3(mouseDir.x, mouseDir.y, 0),
                Quaternion.Euler(new Vector3(0, 0, angle)));

            arrow.GetComponent<Rigidbody2D>().velocity = mouseDir * ArrowSpeed;
        }

        // Tool swapping
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(Holding == "instrument")
            {
                Holding = "bow";
                hand.sprite = Sprites[3];
            }
            else
            {
                Holding = "instrument";
                hand.sprite = Sprites[0];
            }
        }

        // Just make sure player stops playing music
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            //PlayingMusic = false;
            Sound.mute = true;
            // stop instrument sound as well
        }
    }
}
