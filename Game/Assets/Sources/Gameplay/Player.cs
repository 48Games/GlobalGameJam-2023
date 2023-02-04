using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public AnimationCurve animationCurve;
    public float dashCooldown;

    // Child objects
    public GameObject shoot;
    public Transform aimpoint;

    private Vector2 velocity = new Vector2();
    private CharacterController controller;

    // Dash
    private float lastKeyTime;
    private float speedMultiplicator = 1;
    private float currentDashCooldown = 0;
    private float dashAnimationPoint = 0;
    private bool inDash = false;
    private bool stopped = false;

    // Shoot
    public float shootCooldown;
    private float currentShootCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Keyframe lastKey = animationCurve[animationCurve.length - 1];
        lastKeyTime = lastKey.time;
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
        if(inDash)
        {
            dashAnimationPoint += Time.deltaTime;
            if(dashAnimationPoint >= lastKeyTime)
            {
                // end of the dash
                speedMultiplicator = 1;
                inDash = false;
                if(stopped)
                {
                    stopped = false;
                    velocity = Vector2.zero;
                }
            }
            else
            {
                speedMultiplicator = animationCurve.Evaluate(dashAnimationPoint);
            }
        }
        controller.Move(new Vector3(velocity.x,0, velocity.y).normalized * speed * speedMultiplicator * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(!inDash)
        {
            velocity = context.ReadValue<Vector2>();
        } else if (context.ReadValue<Vector2>() == Vector2.zero)
        {
            stopped = true;
        } else
        {
            stopped = false;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(currentDashCooldown == 0)
        {
            currentDashCooldown = dashCooldown;
            dashAnimationPoint = 0;
            inDash = true;
            if(velocity.x == 0 && velocity.y == 0)
            {
                // Immobile
                velocity = new Vector2(transform.forward.x, transform.forward.z);
                stopped = true;
            } else
            {
                transform.forward = new Vector3(velocity.normalized.x, 0, velocity.normalized.y);
            }
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
        if(currentShootCooldown == 0 && !inDash)
        {
            GameObject s = Instantiate(shoot);
            s.GetComponent<Shoot>().owner = this;
            s.GetComponent<Transform>().position = aimpoint.position;
            s.GetComponent<Shoot>().SetVelocity(transform.forward);
            currentShootCooldown = shootCooldown;
        }
    }


}
