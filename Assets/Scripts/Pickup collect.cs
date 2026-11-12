using UnityEngine;
using UnityEngine.UI;

public class PickupCollect : MonoBehaviour
{
    //Battery collectiom logic 

    public static int pickupAmount = 0;

    // Holds the images
    public static RawImage hud;

    // Battery sprites 
    [SerializeField] Texture pickup1Tex;
    [SerializeField] Texture pickup2Tex;
    [SerializeField] Texture pickup3Tex;
    [SerializeField] Texture pickup4Tex;
    [SerializeField] Texture pickup0Tex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the gameObject
        hud = gameObject.GetComponentInChildren<RawImage>();

        // Hide the Image on start
        hud.enabled = false;

        // set inital pickup amount
        pickupAmount = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        if(pickupAmount == 1)
        {
            hud.texture = pickup1Tex;

            hud.enabled = true;
        }
        else if(pickupAmount == 2)
        {
            hud.texture = pickup2Tex;
        }
        else if(pickupAmount == 3)
        {
            hud.texture = pickup3Tex;
        }
        else if(pickupAmount == 4)
        {
            hud.texture = pickup4Tex;
        }
        else
        {
            hud.texture = pickup0Tex;
        }

    }
    
    
}
