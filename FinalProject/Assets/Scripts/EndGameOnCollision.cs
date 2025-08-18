using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed if we want to reload or quit

public class EndGameOnCollision : MonoBehaviour
{
    [Header("Player Tag Setup")]
    public string playerTag = "Player"; // make sure your player GameObject is tagged "Player"

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            Debug.Log("Player caught the tank! Game Over.");
            EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player caught the tank! Game Over.");
            EndGame();
        }
    }

    void EndGame()
    {
        // Option 1: Restart current scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Option 2: Quit the game (only works in a build, not in editor)
        Application.Quit();

        // Option 3: Just stop time (freeze game)
        Time.timeScale = 0f;
    }
}
