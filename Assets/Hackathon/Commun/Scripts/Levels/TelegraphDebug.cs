using UnityEngine;

public class TelegraphDebug : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision avec : " + collision.collider.name);
    }
}
