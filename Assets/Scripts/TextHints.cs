using UnityEngine;
using UnityEngine.UI;

public class TextHints : MonoBehaviour
{   
    // text hint logic 
    public static string message; // Message content 

    static Text textHint; // Onscrren text object 

    public static bool textOn;

    private float timer = 0.0f;

    [SerializeField] float textOnTime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textHint = GetComponent <Text>();

        timer = 0.0f;

        textOn = false;

        textHint.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(textOn)
        {
            textHint.enabled = true;

            textHint.text = message;
            
            timer += Time.deltaTime;
        }

        if(timer> textOnTime)
        {
            textOn = false;

            textHint.enabled = false;

            timer = 0.0f;
        }
    }
}
