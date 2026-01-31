using UnityEngine;
using System.Collections;

public class TeleportCailloux : MonoBehaviour
{
    [SerializeField] private float hauteurAuDessus = 2f;
    [SerializeField] private float delaiEntreTP = 0.3f;

    // Appelle cette méthode pour TP tous les cailloux au-dessus du panier avec un délai
    public void TeleportCaillouxAuDessus()
    {
        StartCoroutine(TeleportationProgressive());
    }

    private IEnumerator TeleportationProgressive()
    {
        GameObject[] cailloux = GameObject.FindGameObjectsWithTag("Caillou");
        Vector3 positionPanier = transform.position;

        foreach (GameObject caillou in cailloux)
        {
            caillou.transform.position = positionPanier + Vector3.up * hauteurAuDessus;
            yield return new WaitForSeconds(delaiEntreTP);
        }
    }
}