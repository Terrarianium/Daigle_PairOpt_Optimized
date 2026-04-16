using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text; 
    [SerializeField] UIManager ui;

    private float time = 0; // Set private and initialized to 0

    /* Subscribe and unsubscribe to the OnActiveGame event.
     */
    private void OnEnable()
    {
        UIManager.OnActiveGame += StartUpdateText;
    }
    private void OnDisable()
    {
        UIManager.OnActiveGame -= StartUpdateText;
    }

    /* Start or stop the timer based on the game state. If the game is not active, also reset the timer and timer text.
     */
    private void StartUpdateText(bool bIsupdating) 
    { 
        if (bIsupdating) 
        { 
            StartCoroutine(UpdateText()); 
        } 
        else 
        {
            StopAllCoroutines();
            time = 0; // Reset time when the game is not active
            text.text = "Timer: 0"; // Reset text to show 0
        }
    }

    /* Updates the timer and the text every 0.1 seconds while the game is active.
     */
    private IEnumerator UpdateText()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            time += 0.1f;
            text.text = "Timer: " + time.ToString("F1");
        }
    }
}
