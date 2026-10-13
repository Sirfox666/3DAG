using UnityEngine;

public class Exp : MonoBehaviour
{
    private int num1 = 4;

    private string message = "Your username is:";

    [SerializeField] private string userName = "Jacob";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Debug.Log(num1);
       UsernameOutput(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UsernameOutput()
    {
        Debug.Log(message + " " + userName + ".");
    }   


}

