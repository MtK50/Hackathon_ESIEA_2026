using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionZone : MonoBehaviour
{
    
    [SerializeField] private float delayBeforeLoad = 0f; // Délai optionnel en secondes

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si c'est le joueur qui entre
        Debug.Log("Trigger entered by: " + other.name);
        if (!hasTriggered && (other.CompareTag("Player") || other.name.Contains("OVR") || other.name.Contains("Player")))
        {
            hasTriggered = true;

            if (delayBeforeLoad > 0)
            {
                Invoke("LoadScene", delayBeforeLoad);
            }
            else
            {
                LoadScene();
            }
        }
    }

    private void LoadScene()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}