using UnityEngine;
using UnityEngine.SceneManagement;

public class VRButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float pressDistance = 0.1f;
    [SerializeField] private Color normalColor = Color.red;
    [SerializeField] private Color pressedColor = Color.green;

    private Vector3 initialPosition;
    private Renderer buttonRenderer;
    private bool hasActivated = false;

    private void Start()
    {
        initialPosition = transform.position;
        buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = normalColor;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // N'importe quel objet qui touche le bouton l'active
        if (!hasActivated)
        {
            ActivateButton();
        }
    }

    // Alternative avec trigger (choisissez l'une des deux méthodes)
    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated)
        {
            ActivateButton();
        }
    }

    private void ActivateButton()
    {
        hasActivated = true;

        // Enfonce le bouton visuellement
        transform.position = initialPosition - transform.forward * pressDistance;

        // Change la couleur
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = pressedColor;
        }

        // Optionnel : ajouter un son
        // AudioSource.PlayClipAtPoint(buttonSound, transform.position);

        // Charge la scène après un court délai
        Invoke("LoadScene", 0.5f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}