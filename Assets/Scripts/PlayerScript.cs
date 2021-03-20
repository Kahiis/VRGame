using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Author: Niko Kahilainen
/// A basic player controller. Assigns player's hands properly for the game
/// Based on a tutorial series by Valem
/// https://www.youtube.com/watch?v=gGYtahQjmWQ
/// </summary>
public class PlayerScript : MonoBehaviour
{
    [Range(0, 0.98f)]
    public float triggerTreshold = 0.8f;
    public float shootCooldown = 0.5f;
    float leftHandTimer;
    float RightHandTimer;

    InputDevice left;
    InputDevice right;

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public GameObject bulletPrefab;
    public float bulletForce = 10f;
    [Min(0)]
    public float bulletLifetime = 5f;
    public AudioSource hitSound;
    public AudioSource deathSound;

    public int hitPoints = 10;
    [HideInInspector]
    public bool alive = true;


    // Start is called before the first frame update
    void Start()
    {
        // A silly way of arranging the controllers properly. Only tested with the Oculus Rift S
        Debug.Log("Starting playerscript");
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics controller = InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(controller, devices);
        List<InputDevice> temp = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, temp);
        left = temp[0];

        List<InputDevice> temp1 = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, temp1);
        right = temp1[0];
    }

    // Update is called once per frame
    void Update()
    {
        leftHandTimer += Time.deltaTime;
        RightHandTimer += Time.deltaTime;

        right.TryGetFeatureValue(CommonUsages.trigger, out float rightValue);
        if (rightValue > triggerTreshold)
        {
            Debug.Log("pressing right button with value " + rightValue);
            if (RightHandTimer > shootCooldown)
            {
                RightHandTimer = 0f;
                Fire(rightHandTransform);
            }
        }

        left.TryGetFeatureValue(CommonUsages.trigger, out float leftValue);
        if (leftValue > triggerTreshold)
        {
            Debug.Log("pressing left button with value " + leftValue);
            if (leftHandTimer > shootCooldown)
            {
                leftHandTimer = 0f;
                Fire(leftHandTransform);
            }
        }
    }

    /// <summary>
    /// Fires a bullet
    /// </summary>
    /// <param name="hand">the transform of the hand we want to shoot the bullet from</param>
    public void Fire(Transform hand)
    {
        Vector3 spawnPos = hand.transform.position;// + (Vector3.forward * 0.5f);// + (Vector3.forward * 2);
        Vector3 spawnDirection = hand.transform.forward;
        Quaternion rotation = hand.transform.rotation;
        spawnPos = spawnPos + spawnDirection * 0.3f;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = bullet.transform.forward;
        rb.AddForce(direction * bulletForce);
        Destroy(bullet, bulletLifetime);
    }

    /// <summary>
    /// Handling getting hit by a baddie
    /// </summary>
    public void HitByEnemy()
    {
        if (alive)
        {
            hitPoints--;
            if (hitPoints <= 0)
            {
                alive = false;
                deathSound.Play();
                return;
            }
            shootCooldown = shootCooldown * 0.9f; // the shot cooldown decrease each time you get hit!
            hitSound.Play();
        }
    }
}
