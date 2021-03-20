using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Debug.Log("Restarting");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
