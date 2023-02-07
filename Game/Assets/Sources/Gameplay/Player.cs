using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using Visuals;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Le Son")]
    [Tooltip("Audio source for footsteps, jump, etc...")]
    public AudioSource AudioSource;

    public float FootstepSfxFrequency = 1f;
    public AudioClip[] FootstepSfx;
    public Vector2 VoiceSfxFrequency = new Vector2(5f,15f);
    public AudioClip[] VoicesSfx;
    public AudioClip DashSfx;
    public AudioClip AttackSfx;
    public AudioClip RootedSfx;
    public AudioClip DeathSfx;
    public AudioClip BuffSmallSfx;
    public AudioClip BuffBigSfx;
    public AudioClip BuffHugeSfx;

    float m_FootstepDistanceCounter;
    float m_VoiceCounter;


    [Header("Les trucs de Thomas")]
    public float speed;
    public float acceleration;
    public AnimationCurve animationCurve;
    public float dashCooldown;
    public float rootDuration;

    // Child objects
    public GameObject shoot;
    public Transform aimpoint;

    [Header("Visuals")]
    public Color[] playerColors;
    public GameObject rootVisualPrefab;
    public GameObject deathVFXPrefab;

    private Rigidbody rigidbody;

    // Dash
    private float lastKeyTime;
    private float currentDashCooldown = 0;
    private float dashAnimationPoint = 0;
    private bool inDash = false;

    public bool Rooted { get; private set; }

    // Shoot
    public float shootCooldown;
    private float currentShootCooldown = 0;

    private Vector3 targetVelocity;
    private Vector3 currentVelocity;
    private Vector3 dashDirection;

    private GameObject rootVisual;
    private CharacterVisual characterVisual;

    // Spawner
    public Vector3 Spawn { get; set; }

    // Buff
    public bool buffspeed = false;
    public float buffspeedmultiplier;
    public bool buffshoot = false;

    // Keyboard
    Plane m_Plane;



    public int PlayerID { get; private set; }

    // Animator
    [SerializeField] private Animator animator;

    // GameManager
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Keyframe lastKey = animationCurve[animationCurve.length - 1];
        lastKeyTime = lastKey.time;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.PlayerRegister(this.gameObject);

        //Create a new plane with normal (0,0,1) at the position away from the camera you define in the Inspector. This is the plane that you can click so make sure it is reachable.
        m_Plane = new Plane(Vector3.up, Vector3.zero);

    }

    public void SetupPlayer(int playerID)
    {
        PlayerID = playerID;
        characterVisual = GetComponentInChildren<CharacterVisual>();
        characterVisual.SetCharacterColor(playerColors[playerID]);
    }

    public void Root()
    {
        if (Rooted) return;
        AudioSource.PlayOneShot(RootedSfx);
        StartCoroutine(RootCoroutine());
    }

    private IEnumerator RootCoroutine()
    {
        Rooted = true;
        rigidbody.isKinematic = true;
        animator.SetBool("Rooted", true);
        rootVisual = Instantiate(rootVisualPrefab);
        rootVisual.transform.position = transform.position;
        float progress = 0.0f;
        while (progress < rootDuration)
        {
            var scale = Mathf.Clamp01(progress * rootDuration / 0.2f);
            rootVisual.transform.localScale = Vector3.one * scale * 1.5f;
            yield return null;
            progress += Time.deltaTime;
        }
        Rooted = false;
        animator.SetBool("Rooted", false);
        rigidbody.isKinematic = false;
        Destroy(rootVisual);
        rootVisual = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsAbleToMove())
        {
            currentVelocity = Vector3.zero;
            rigidbody.velocity = currentVelocity;
            return;
        }
            

        if (currentShootCooldown > 0)
        {
            currentShootCooldown -= Time.deltaTime;
            if (currentShootCooldown < 0)
            {
                currentShootCooldown = 0;
            }
        }
        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
            if(currentDashCooldown < 0)
            {
                currentDashCooldown = 0;
            }
        }

        if (Rooted)
        {
            currentVelocity = Vector3.zero;
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
            if (buffspeed) currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity * buffspeedmultiplier, Time.deltaTime * acceleration);
            if (inDash)
            {
                dashAnimationPoint += Time.deltaTime;
                if (dashAnimationPoint >= lastKeyTime)
                {
                    // end of the dash
                    characterVisual.ShowTrail(false);
                    inDash = false;
                    currentVelocity = Vector2.zero;
                }
                else
                {
                    currentVelocity = speed * animationCurve.Evaluate(dashAnimationPoint) * dashDirection;
                }
            }
        }

        rigidbody.velocity = currentVelocity;
        animator.SetFloat("Speed", currentVelocity.magnitude);

        // footsteps sound
        if (m_FootstepDistanceCounter >= 1f / FootstepSfxFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            AudioSource.PlayOneShot(FootstepSfx[(int)(Random.value*(FootstepSfx.Length -1))]);
        }
        m_FootstepDistanceCounter += rigidbody.velocity.magnitude * Time.deltaTime;

        // voices sound
        if (m_VoiceCounter <= 0f)
        {
            m_VoiceCounter = Random.Range(VoiceSfxFrequency.x,VoiceSfxFrequency.y);
            AudioSource.PlayOneShot(VoicesSfx[(int)(Random.value*(VoicesSfx.Length -1))]);
        }
        m_VoiceCounter -= Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 newVelocity = context.ReadValue<Vector2>();
        targetVelocity = new Vector3(newVelocity.x, 0, newVelocity.y) * speed;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(currentDashCooldown == 0)
        {
            currentDashCooldown = dashCooldown;
            dashAnimationPoint = 0;
            inDash = true;
            characterVisual.ShowTrail(true);
            if (currentVelocity.x == 0 && currentVelocity.z == 0)
            {
                // Immobile
                // velocity = new Vector2(transform.forward.x, transform.forward.z);
                // stopped = true;
            } else
            {
                transform.forward = currentVelocity;
                dashDirection = currentVelocity.normalized;
            }

            animator.SetTrigger("Dash");
            AudioSource.PlayOneShot(DashSfx);
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if(!inDash)
        {
            Vector3 v = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
            if (v != Vector3.zero)
            {
                if(context.control.device.name.Contains("Mouse"))
                {
                    Ray ray = Camera.main.ScreenPointToRay(v);

                    //Initialise the enter variable
                    float enter = 0.0f;

                    if (m_Plane.Raycast(ray, out enter))
                    {
                        //Get the point that is clicked
                        v = ray.GetPoint(enter);
                        int mul = 1;
                        if ((v - transform.position).x < 0)
                        {
                            mul = -1;
                        }
                        float angle = Vector3.Angle((v - transform.position).normalized, new Vector3(0,0,1)) * mul;
                        Debug.Log((v - transform.position).normalized);
                        transform.eulerAngles = new Vector3(0, angle, 0);
                    }
                }
                else
                {
                    transform.forward = v;
                }
            }
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (currentShootCooldown <= 0 && !inDash)
        {
            animator.SetTrigger("Attack");
            currentShootCooldown = shootCooldown;
            AudioSource.PlayOneShot(AttackSfx);
        }
    }

    public void ShootEvent()
    {
        GameObject s = Instantiate(shoot);
        s.GetComponent<Shoot>().owner = this;
        s.transform.position = aimpoint.position;
        s.transform.forward = transform.forward;
        s.GetComponent<Shoot>().SetVelocity(transform.forward);
    }

    public void Death()
    {
        if(rootVisual != null)
        {
            Rooted = false;
            animator.SetBool("Rooted", false);
            rigidbody.isKinematic = false;
            Destroy(rootVisual);
            rootVisual = null;
        }

        AudioSource.PlayOneShot(DeathSfx);
        this.gameObject.SetActive(false);
        gameManager.PlayerDie(this.gameObject);

        var deathVfx = Instantiate(deathVFXPrefab);
        deathVfx.transform.position = transform.position;
        deathVfx.GetComponent<DeathFXVisual>().SetColor(characterVisual.Color);
        Destroy(deathVfx, 1.0f);
    }


}
