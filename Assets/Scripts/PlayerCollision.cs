using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    //Door Animator
    [SerializeField] private GameObject door; // An open slot in the Inspector to drag the hinge object into

    Animator doorAnimator;

    // Door timer 
    [Header("Door timer")]
    private bool startDoorTimer = false;
    private float doorTimer = 0.0f;
    [SerializeField] private float doorOpenTime = 3.0f;

    // Door Sounds
    [SerializeField] private AudioClip doorOpenSound; 
    [SerializeField] private AudioClip doorShutSound;

    private new AudioSource audio; // will hold the AudioSource component

    private bool doorIsOpen; // To determine current state of door 

    // Battery sound 
    [SerializeField] private AudioClip pickupCollectSound; 

    //spawn point
    [SerializeField] private Transform spawnPoint; 

    private bool isRespawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Run a door timer 
        if(startDoorTimer)
        {
            doorTimer += Time.deltaTime; 
        }

        //Automaatically shut door after timer
        if(doorTimer >= doorOpenTime)
        {
            ShutDoor(); // This calls the custom method below

            doorTimer = 0.0f;
        }
    }

    // Collision detection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // check if the player hits an object with the houseDoor tag
        if(hit.gameObject.CompareTag("houseDoor") && PickupCollect.pickupAmount >= 4)
        {
            // print("Hit the door!");
            OpenDoor();

            PickupCollect.hud.enabled = false; 
        }
        else if(hit.gameObject.CompareTag("houseDoor") && PickupCollect.pickupAmount < 4)
        {
            PickupCollect.hud.enabled = true;

            TextHints.message = "This door needed power to open...";

            TextHints.textOn = true;
        }

        if(hit.gameObject.CompareTag("water") && !isRespawning)
        {

            isRespawning = true;

            TextHints.message = "Can't go there!";

            TextHints.textOn = true;

            StartCoroutine(Respawn(2.0f));

            Time.timeScale = 0;
        }


    }

    // Custom open door method
    private void OpenDoor()
    {
        if (!doorIsOpen);
        {
            audio.PlayOneShot(doorOpenSound);

            doorIsOpen = true;
        }
        audio.PlayOneShot(doorOpenSound);

        // set Animator parameter 
        doorAnimator.SetBool("doorOpen", true);

        startDoorTimer = true; 
    }

    // custom shut door method 
    private void ShutDoor()
    {
        if(doorIsOpen)
        {
            audio.PlayOneShot(doorShutSound);

            doorIsOpen = false;

        }
        
        // Set Animator parameter
        doorAnimator.SetBool("doorOpen", false);
    }

    // pickup collection
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag("pickup"))
        {
            PickupCollect.pickupAmount++;

             audio.PlayOneShot(pickupCollectSound);

             Destroy(coll.gameObject);
        }
    }

     private IEnumerator Respawn(float delay)   
    {
        yield return new WaitForSecondsRealtime(delay);
    
        transform.position = spawnPoint.position;

        isRespawning = false;

        Time.timeScale = 1;
    }

}
