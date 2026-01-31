using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Nom de la scène à charger
    [SerializeField] private bool isActivated = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si l'objet qui entre en collision a le tag "ThrowableObject"
        if (collision.gameObject.CompareTag("ThrowableObject") && !isActivated)
        {
            ActivateButton();
        }
    }

    private void ActivateButton()
    {
        isActivated = true;

        // Animation ou effet visuel (optionnel)
        GetComponent<Renderer>().material.color = Color.green;

        // Charge la nouvelle scène après un court délai
        Invoke("LoadScene", 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
        const string debugMessage = "Scene loading is disabled for debugging purposes.";
        Debug.Log(debugMessage);
    }
}