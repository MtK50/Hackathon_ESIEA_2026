using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class TelegraphPuzzle : MonoBehaviour
{
    [Header("Temps")]
    public float letterDelay = 1.2f;
    public float inputCooldown = 0.25f;

    private bool canInput = true;

    [Header("References")]
    public HandGrabInteractable hammer;
    public HandGrabInteractable hammer2;

    public WaterPumpController pump;
    public int remp = 0;
    public AudioSource audioSource;
    public AudioClip pumpSound;

    private List<string> targetLetters = new List<string>()
    {
        "-.-.",
        "---",
        "..-",
        ".-.",
        "-"
    };

    private int currentLetterIndex = 0;
    private string currentLetterInput = "";

    private Coroutine letterCoroutine;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.tag + " " + collision.gameObject.name);
        if (collision.tag == "marto")
        { 
            if (!canInput) return;
            StartCoroutine(RegisterInput("-"));
            Bridge.Instance?.SendLine("z:TOUCH:z:z");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!canInput) return;

        // ----- Marteau -----
        var interactable = collision.collider.GetComponentInParent<HandGrabInteractable>();

        if (interactable == hammer || interactable == hammer2)
        {
            StartCoroutine(RegisterInput("-"));
            Bridge.Instance?.SendLine("z:TOUCH:z:z");
            return;
        }

        // // ----- Main tracking Meta -----
        // var hand = collision.collider.GetComponentInParent<OVRHand>();
        //
        // if (hand != null && hand.IsTracked)
        // {
        //     StartCoroutine(RegisterInput("."));
        //     Bridge.Instance?.SendLine("a:TOUCH:z:z");
        // }
    }

    public void SendMorseCodeDot()
    {
        if (!canInput) return;
        StartCoroutine(RegisterInput("."));
        Bridge.Instance?.SendLine("a:TOUCH:z:z");
    }
    
    
    IEnumerator RegisterInput(string symbol)
    {
        canInput = false;

        AddSymbol(symbol);

        yield return new WaitForSeconds(inputCooldown);
        canInput = true;
    }

    void AddSymbol(string symbol)
    {
        currentLetterInput += symbol;

        Debug.Log("Lettre en cours : " + currentLetterInput);

        if (letterCoroutine != null)
            StopCoroutine(letterCoroutine);

        letterCoroutine = StartCoroutine(ValidateLetterAfterDelay());
    }

    IEnumerator ValidateLetterAfterDelay()
    {
        yield return new WaitForSeconds(letterDelay);
        ValidateLetter();
    }

    void ValidateLetter()
    {
        string expected = targetLetters[currentLetterIndex];

        if (currentLetterInput == expected)
        {
            Debug.Log("Bonne lettre !");

            Bridge.Instance?.SendLine("b:TOUCH:z:z");
            Bridge.Instance?.SendLine("b:TOUCH:z:z");
            Bridge.Instance?.SendLine("b:TOUCH:z:z");
            remp += 5;
            pump.FillSettings(remp);
            pump.Fill(true);

            currentLetterIndex++;
            currentLetterInput = "";

            if (currentLetterIndex >= targetLetters.Count)
            {
                Debug.Log("MOT COMPLET VALIDE !");
                ResetPuzzle();
                StartCoroutine(PumpRoutine());

                IEnumerator PumpRoutine()
                {
                    pump.PulseSettings(0.15f, 0.15f);
                    pump.Pulse(true);
                    audioSource.PlayOneShot(pumpSound, 2f);

                    yield return new WaitForSeconds(5f); // attente 5 secondes

                    pump.Pulse(false);

                    remp = 0;
                    pump.FillSettings(remp);
                    pump.Fill(true);
                }


                LevelManager.Instance.currentLevel.OnCompleteLevel();

            }
        }
        else
        {
            Debug.Log("Mauvaise lettre ! RESET COMPLET");

            Bridge.Instance?.SendLine("d:TOUCH:z:z");
            ResetPuzzle();
            remp = 0;
            pump.FillSettings(remp);
            pump.Fill(true);
        }
    }

    void ResetPuzzle()
    {
        currentLetterIndex = 0;
        currentLetterInput = "";
    }
}
