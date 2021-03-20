using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Niko Kahilainen
/// A simple zombie baddie
/// </summary>
public class BaddieScript : MonoBehaviour
{
    public Transform target;
    public float movespeed;
    public AudioSource hitSound;
    public AudioSource eatSound;
    BaddieSpawnerScript bscript;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        if (!target)
            Debug.LogError("No target given!");

        if (!hitSound)
            Debug.LogError("No hitsound given!");

        bscript = GetComponentInParent<BaddieSpawnerScript>();

        transform.LookAt(target.position);
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Move the baddie in a smooth zombielike fashion
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Time.deltaTime * movespeed);
    }

    /// <summary>
    /// Handling baddie death, as in the baddie was pushed out of bounds
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayArea"))
        {
            Debug.Log("Fug, i em dead :D");
            bscript.BaddieKilled();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// handling the collision between the bullet and the baddie
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            hitSound.Play();
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// handling the collision between the player and the baddie
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bscript.HitPlayer(this.gameObject);
        }
    }
}
