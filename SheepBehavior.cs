using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBehavior : MonoBehaviour
{
    public GameObject Target;
    public float Speed = 0.1f;          // Movement per fixedUpdate
    public float HearingDistance = 8f;  // When to begin following a target
    public float StopDistance = 2f;     // How close to target to stop
    public float CatchUpDistance = 3f;  // How far the target should go before continuing to follow 
    public bool followMusic;

    private Player playerScript;
    private bool waiting;
    private Bobbing visual;
    private SpriteRenderer sprite;

    public void Greet(GameObject player)
    {
        if(player != null) playerScript = player.GetComponent<Player>();
        Target = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // remove?
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        visual = GetComponent<Bobbing>();
    }

    private void FixedUpdate()
    {
        if(Target == null)
        {
            sprite.color = Color.red;
            return;
        }

        Vector3 direction = Target.transform.position - this.transform.position;

        // Do nothing if player is supposed to be playing music but isn't.
        // Do nothing if target is too far away.
        // Stop when when close enough
        if((followMusic && !playerScript.PlayingMusic)
            || (direction.magnitude > HearingDistance)
            || (direction.magnitude <= StopDistance))
        {
            waiting = true;
            visual.Bob = false;
            return;
        }

        // Don't move until target leaves
        if(waiting && direction.magnitude <= CatchUpDistance)
        {
            visual.Bob = false;
            return;
        }
        else
        {
            waiting = false;
            visual.Bob = true;
            sprite.flipX = (direction.x < 0);
            this.transform.position += direction.normalized * Speed;
        }
		
		var dz = 0.1f;
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * dz);
    }
}
