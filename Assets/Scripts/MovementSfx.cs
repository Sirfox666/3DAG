using UnityEngine;
using StarterAssets;

// Provided as is to students to use (optional)

[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(CharacterController))]
public class MovementSfx : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource jumpLandingSource; // Optional: separate source for jump and landing volume or mixer control
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;

    [Header("Volume")]
    [Range(0, 1)] [SerializeField] private float footstepVolume = 0.4f;
    [Range(0, 1)] [SerializeField] private float jumpVolume = 0.5f;
    [Range(0, 1)] [SerializeField] private float landVolume = 0.5f;

    [Header("Timing")]
    [SerializeField] private float stepDistance = 1.6f;   // Meters walked per footstep
    [SerializeField] private float minAirTime = 0.2f;     // Shorter drops make no landing sound
    [SerializeField] private float startupSilence = 1f;   // Stay quiet while spawning in

    private FirstPersonController _fpc;
    private CharacterController _controller;
    private bool _wasGrounded;
    private float _distance;
    private float _airTime;
    private float _quietUntil;
    private int _lastClip = -1;

    void Start()
    {
        _fpc = GetComponent<FirstPersonController>();
        _controller = GetComponent<CharacterController>();

        if (!footstepSource) footstepSource = GetComponent<AudioSource>();
        if (!jumpLandingSource) jumpLandingSource = footstepSource;

        _wasGrounded = _fpc.Grounded;
        _quietUntil = Time.time + startupSilence;
    }

    void Update()
    {
        bool grounded = _fpc.Grounded;

        if (grounded)
        {
            if (!_wasGrounded) Land();
            Footsteps();
        }
        else
        {
            // Left the ground going up = a jump
            if (_wasGrounded && _controller.velocity.y > 0)
                Play(jumpLandingSource, jumpClip, jumpVolume);
            _airTime += Time.deltaTime;
        }

        _wasGrounded = grounded;
    }

    void Land()
    {
        if (_airTime >= minAirTime) Play(jumpLandingSource, landClip, landVolume);
        _airTime = 0;
        _distance = 0;
    }

    void Footsteps()
    {
        // Step by distance, so sprinting speeds up the steps on its own
        Vector3 v = _controller.velocity;
        v.y = 0;
        _distance += v.magnitude * Time.deltaTime;
        if (_distance < stepDistance) return;

        _distance = 0;
        if (footstepClips.Length == 0) return;

        // Pick a random filled slot, never repeating the last clip
        int start = Random.Range(0, footstepClips.Length);
        for (int n = 0; n < footstepClips.Length; n++)
        {
            int i = (start + n) % footstepClips.Length;
            if (!footstepClips[i] || i == _lastClip) continue;

            _lastClip = i;
            Play(footstepSource, footstepClips[i], footstepVolume);
            return;
        }

        // Only one filled slot, so repeat it
        if (_lastClip >= 0 && footstepClips[_lastClip])
            Play(footstepSource, footstepClips[_lastClip], footstepVolume);
    }

    void Play(AudioSource source, AudioClip clip, float volume)
    {
        if (source && clip && Time.time >= _quietUntil)
            source.PlayOneShot(clip, volume);
    }
}
