using UnityEngine;
using UnityEngine.InputSystem;


public class TestSerial : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("Test A");
            Bridge.Instance?.SendLine("a:TOUCH:z:z");
        }

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            Debug.Log("Test Z");
            Bridge.Instance?.SendLine("z:TOUCH:z:z");
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            Debug.Log("Test D");
            Bridge.Instance?.SendLine("d:TOUCH:z:z");
        }
    }
}
