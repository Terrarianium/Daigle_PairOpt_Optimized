using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text; 
    [SerializeField] float time; 
    [SerializeField] UIManager ui;

    // Update is called once per frame
    void Update()
    {
        if (ui.startGame) 
        { 
            time += Time.deltaTime; 
            UpdateText(); 
        }
    }

    void UpdateText() 
    { 
        text.text = time.ToString("F0"); 
    }
}
