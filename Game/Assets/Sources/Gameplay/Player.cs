using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using Visuals;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
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

    private GameObject visual;

    // Spawner
    public Vector3 Spawn { get; set; }


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
    }

    public void SetupPlayer(int playerID)
    {
        PlayerID = playerID;
        var characterVisual = GetComponentInChildren<CharacterVisual>();
        characterVisual.SetCharacterColor(playerColors[playerID]);
    }

    public void Root()
    {
        if (Rooted) return;
        StartCoroutine(RootCoroutine());
    }

    private IEnumerator RootCoroutine()
    {
        Rooted = true;
        rigidbody.isKinematic = true;
        animator.SetBool("Rooted", true);
        visual = Instantiate(rootVisualPrefab);
        visual.transform.position = transform.position;
        float progress = 0.0f;
        while (progress < rootDuration)
        {
            var scale = Mathf.Clamp01(progress * rootDuration / 0.2f);
            visual.transform.localScale = Vector3.one * scale * 1.5f;
            yield return null;
            progress += Time.deltaTime;
        }
        Rooted = false;
        animator.SetBool("Rooted", false);
        rigidbody.isKinematic = false;
        Destroy(visual);
        visual = null;
    }

    // Update is called once per frame
    void Update()
    {
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
            if (inDash)
            {
                dashAnimationPoint += Time.deltaTime;
                if (dashAnimationPoint >= lastKeyTime)
                {
                    // end of the dash
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
            if(currentVelocity.x == 0 && currentVelocity.z == 0)
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
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if(!inDash)
        {
            Vector2 v = context.ReadValue<Vector2>();
            if (v != Vector2.zero)
            {
                transform.forward = new Vector3(v.x, 0, v.y);
            }
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (currentShootCooldown <= 0 && !inDash)
        {
            animator.SetTrigger("Attack");
            currentShootCooldown = shootCooldown;
        }
    }

    public void ShootEvent()
    {
        GameObject s = Instantiate(shoot);
        s.GetComponent<Shoot>().owner = this;
        s.GetComponent<Transform>().position = aimpoint.position;
        s.GetComponent<Shoot>().SetVelocity(transform.forward);
    }

    public void Death()
    {
        if(visual != null)
        {
            Rooted = false;
            animator.SetBool("Rooted", false);
            rigidbody.isKinematic = false;
            Destroy(visual);
            visual = null;
        }
        this.gameObject.SetActive(false);
        gameManager.PlayerDie(this.gameObject);
    }

}
