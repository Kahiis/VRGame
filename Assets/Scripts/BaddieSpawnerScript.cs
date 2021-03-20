using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Niko Kahilainen
/// A script which manages the spawning of baddies
/// </summary>
public class BaddieSpawnerScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public Transform target;
    public GameObject baddie;
    [Min(0f)]
    // public float incrementalSpeedIncrease = 0.1f;
    public List<GameObject> spawns;
    int kills = 0;
    public GameObject killsUI;
    public GameObject HealthUI;
    public GameObject RestartUI;
    // int enemyCount = 1;
    // float speed;
    // Start is called before the first frame update
    void Start()
    {
        if (!baddie)
        {
            Debug.LogError("Missing baddie prefab!");
            return;
        }

        if (!playerScript)
        {
            Debug.LogError("Missing playerscript!");
            return;
        }

        if (!killsUI)
        {
            Debug.LogError("Missing Kill UI!");
            return;
        }

        if (!HealthUI)
        {
            Debug.LogError("Missing Health UI");
            return;
        }

        if (spawns.Count == 0)
        {
            Debug.LogError("Missing spawns!");
            return;
        } else
        {
            SpawnBaddie();
        }
    }

    public void BaddieKilled()
    {
        kills++;
        // every tenth kill we increase the amount of enemies that can be on the field
        if (kills % 10 == 0)
        {
            // speed += incrementalSpeedIncrease;
            SpawnBaddie();
        }
        SpawnBaddie();
        killsUI.GetComponent<TextMesh>().text = "Kills: " + kills;
    }

    /// <summary>
    /// Spawns a baddie
    /// </summary>
    void SpawnBaddie()
    {
        // hmm, some well done spaghetti once more
        Vector3 spawnPos = spawns[Random.Range(0, spawns.Count)].transform.position;
        GameObject temp = Instantiate(baddie, spawnPos, Quaternion.identity, this.transform);
        BaddieScript script = temp.GetComponent<BaddieScript>();
        script.target = target; 
    }

    /// <summary>
    /// The enemy hit the player so we will respawn it at a random spawn
    /// </summary>
    /// <param name="hitter">the badddie which hit the player</param>
    public void HitPlayer(GameObject hitter)
    {
        Vector3 randomPosition = spawns[Random.Range(0, spawns.Count)].transform.position;
        hitter.transform.position = randomPosition;
        playerScript.HitByEnemy();
        HealthUI.GetComponent<TextMesh>().text = "Health: " + playerScript.hitPoints;
        if (!playerScript.alive)
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            RestartUI.SetActive(true);
        }
    }
}
