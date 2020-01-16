using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class WolfBehavior : MonoBehaviour
{
    public GameObject Target;
    public float Speed = 0.1f; // Movement per fixedUpdate
    public float KillDuration = 3;

    private float killStart;
    private bool waiting;
    private Bobbing visual;
    private SpriteRenderer sprite;

    private static GameObject Player;
    private static GameObject[] Sheep;

    void Start()
    {
        visual = GetComponent<Bobbing>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Target != null) return; // Chase only the first target found.
        Player = GameObject.FindGameObjectWithTag("Player");

        if(collision.name.Contains("Sheep")
            || collision.name.Contains("Player"))
        {
            Target = collision.gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Sheep = GameObject.FindGameObjectsWithTag("Sheep");

        if(collision.gameObject == Target)
        {
            // Don't destroy the player. That's bad.
            if(collision.gameObject != Player)
            {
                Destroy(Target);
            }
            else // Murderize a random sheep instead.
            {
                if(Sheep.Length == 1)
                {
                    SceneManager.LoadScene("Game Over Scene");
                }
                else //if(Sheep.Length != 0)
                {
                    var r = Random.Range(0, Sheep.Length - 1);
                    Debug.Log(r);
                    Destroy(Sheep[r]);
                }
            }

            Target = null;
            visual.Bob = false;
            waiting = true;
            killStart = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if(waiting && Time.time < (killStart + KillDuration)
            || Target == null) return;

        visual.Bob = true;
        Vector3 direction = Target.transform.position - this.transform.position;
        sprite.flipX = (direction.x > 0);
        this.transform.position += direction.normalized * Speed;
		
		var dz = 0.1f;
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * dz);
    }
}
