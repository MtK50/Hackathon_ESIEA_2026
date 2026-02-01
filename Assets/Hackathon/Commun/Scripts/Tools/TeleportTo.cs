using UnityEngine;

public class TeleportTo : MonoBehaviour
{
    public void Teleport(Transform target)
    {
        if (target != null)
        {
            GameManager.Instance.playerTransform.position = target.position;
        }
    }
}
