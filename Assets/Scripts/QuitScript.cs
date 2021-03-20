using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Niko Kahilainen
/// A super silly way to exit a vr game
/// </summary>
public class QuitScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }
}
