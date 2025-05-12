using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EditedFirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    private Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    public Animator animator;

    void Awake()
    {
        // Get the rigidbody on this.
        rb = GetComponent<Rigidbody>();
        // animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        float sideInput = Input.GetAxis("Horizontal");
        float forwardInput = Input.GetAxis("Vertical");

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(sideInput, forwardInput).normalized * targetMovingSpeed;

        // Apply animation
        animator.SetFloat("WalkDirection", forwardInput);
        animator.SetFloat("TurnDirection", sideInput);

        // Apply movement.
        rb.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.y);
    }
}