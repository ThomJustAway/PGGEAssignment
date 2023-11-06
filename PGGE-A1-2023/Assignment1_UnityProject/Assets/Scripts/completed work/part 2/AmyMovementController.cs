using PGGE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmyMovementController : MonoBehaviour
{
    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;

    public float mWalkSpeed = 1.5f;
    public float mRotationSpeed = 50.0f;
    public bool mFollowCameraForward = false;
    public float mTurnRate = 10.0f;

#if UNITY_ANDROID
    public FixedJoystick mJoystick;
#endif

    private float hInput;
    private float vInput;
    private float speed;
    private bool jump = false;
    private bool crouch = false;
    public float mGravity = -30.0f;
    public float mJumpHeight = 1.0f;
    [Range(0.2f, 1f)]
    public float speedReductionWhenCrouching = 1f;

    private Vector3 mVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        HandleInputs();
        Move();



    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    public void HandleInputs()
    {
        // We shall handle our inputs here.
#if UNITY_STANDALONE
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
        hInput = 2.0f * mJoystick.Horizontal;
        vInput = 2.0f * mJoystick.Vertical;
#endif

        speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        { //prevent players from 
            speed = mWalkSpeed * 2.0f;
        }

        if (crouch)
        {
            speed = mWalkSpeed * speedReductionWhenCrouching;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            crouch = !crouch;
            Crouch();
        }
    }

    public void Move()
    {
        if (mAnimator == null) return;

        if (!crouch && jump)
        {
            Jump();
            jump = false;
        }
        //making sure that if players crouch, they cant jump
        MovementMethod();


        //mAnimator.SetFloat("PosX", 0);
        
    }

    private void MovementMethod()
    {
        if (mFollowCameraForward)
        {
            // rotate Player towards the camera forward.
            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0.0f, eu.y, 0.0f),
                mTurnRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized; //geting the forward of the transform from world space
        forward.y = 0.0f; //remove any upward force
        mCharacterController.Move(forward * vInput * speed * Time.deltaTime);

        if(crouch)
        {//special for crouch
            mAnimator.SetFloat("PosZ", vInput * speed / (2.0f * speed));
        }
        else
        {
            mAnimator.SetFloat("PosZ", vInput * speed / (2.0f * mWalkSpeed));

        }
        mAnimator.SetFloat("PosX", 0);
    }

    void Jump()
    {
        mAnimator.SetTrigger("Jump");
        mVelocity.y += Mathf.Sqrt(mJumpHeight * -2f * mGravity);
    }

    private Vector3 HalfHeight;
    private Vector3 tempHeight;
    void Crouch()
    {
        mAnimator.SetBool("Crouch", crouch);
        if (crouch)
        {
            tempHeight = CameraConstants.CameraPositionOffset;
            HalfHeight = tempHeight;
            HalfHeight.y *= 0.5f;
            CameraConstants.CameraPositionOffset = HalfHeight;
        }
        else
        {
            CameraConstants.CameraPositionOffset = tempHeight;
        }
    }

    void ApplyGravity()
    {
        // apply gravity.
        mVelocity.y += mGravity * Time.deltaTime;
        if (mCharacterController.isGrounded && mVelocity.y < 0)
            mVelocity.y = 0f;
    }
}