using Oculus.Interaction.HandGrab.Recorder;
using UnityEngine;

public class Feedback_caillou : MonoBehaviour
{
    [SerializeField] private WaterPumpController waterPumpController;

    void Start()
    {
        
    }
    void Update()
    {
        OnGrab();
    }

    public void OnGrab()
    {
        if (CompareTag("Caillou") && waterPumpController != null)
        {

            waterPumpController.Fill(true);

           
            Invoke(nameof(StopFeedback), 0.5f);
        }
    }

    private void StopFeedback()
    {
        if (waterPumpController != null)
        {
            waterPumpController.Pulse(false);
        }
    }
}
