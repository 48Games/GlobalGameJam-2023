using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public AnimationCurve animationCurve;
    public float dashCooldown;

    // Child objects
    public GameObject shoot;
    public Transform aimpoint;

    private Vector2 velocity = new Vector2();
    private CharacterController controller;

    // Dash
    private float lastKeyTime;
    private float currentDashCooldown = 0;
    private float dashAnimationPoint = 0;
    private bool inDash = false;

    // Shoot
    public float shootCooldown;
    private float currentShootCooldown = 0;

    private Vector3 targetVelocity;
    private Vector3 currentVelocity;
    private Vector3 dashDirection;

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
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
        if (inDash)
        {
            dashAnimationPoint += Time.deltaTime;
            if(dashAnimationPoint >= lastKeyTime)
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
        controller.Move(currentVelocity * Time.deltaTime);
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
