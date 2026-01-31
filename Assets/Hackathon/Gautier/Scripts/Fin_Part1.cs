using UnityEngine;

public class Fin_Part_1 : MonoBehaviour
{
    private int caillouCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caillou"))
        {
            caillouCount++;
            Debug.Log($"Caillou dans le panier : {caillouCount}/???");

            Destroy(other.gameObject); 

            if (caillouCount >= 9)
            {
                Debug.Log("Fini ?");
            }
        }

    }
}
