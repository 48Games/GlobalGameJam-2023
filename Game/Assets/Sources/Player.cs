using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // IMPLEM DASH:
    // PENSER IMPLEM ROTA
    // dash direction deplacement si mouvement, rotation si immobile
    public float speed;
    public AnimationCurve animationCurve;
    public float dashCooldown;

    private Vector2 velocity = new Vector2();
    private CharacterController controller;

    // Dash
    private float lastKeyTime;
    private float speedMultiplicator = 1;
    private float currentDashCooldown = 0;
    private float dashAnimationPoint = 0;
    private bool inDash = false;

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
        if(currentDashCooldown > 0)
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
            }
            else
            {
                speedMultiplicator = animationCurve.Evaluate(dashAnimationPoint);
            }
        }
        controller.Move(new Vector3(velocity.x,0, velocity.y) * speed * speedMultiplicator * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        Debug.Log("OK");
        if(currentDashCooldown == 0)
        {
            currentDashCooldown = dashCooldown;
            dashAnimationPoint = 0;
            inDash = true;
        }
    }
}
