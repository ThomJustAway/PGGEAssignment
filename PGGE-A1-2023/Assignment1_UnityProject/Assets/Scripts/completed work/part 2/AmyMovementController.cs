using PGGE;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AmyMovementController : MonoBehaviour
{
    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;

    #region values for amy
    public float mWalkSpeed = 1.5f;
    public float mRotationSpeed = 50.0f;
    public bool mFollowCameraForward = false;
    public float mTurnRate = 10.0f;
    public float mGravity = -30.0f;
    public float mJumpHeight = 1.0f;
    public float inputDamper = 1.0f; //this is to reduce the effects of the input 
    public float resistance = 0.3f; //this is to reduce the acceleration 
    public float speedReductionWhenCrouching = 1f;
    #endregion

    #if UNITY_ANDROID
        public FixedJoystick mJoystick;
    #endif

    #region playerInput

    private float hInput;
    private float vInput;
    private float speed;
    private bool jump = false;
    private bool crouch = false;
    [Range(0.2f, 1f)]
    private Vector3 mVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    #endregion

    #region attacking

    [SerializeField] List<AttackSO> combo = new List<AttackSO>();
    private float lastClickTime;
    private float lastComboEnd;
    private int comboCount;
    private readonly string EndComboFunctionName = "EndCombo";
    private bool attack = false;

    #endregion

    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        HandleInputs();
        AmyNextMove();
        ExitAttack();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    public void HandleInputs()
    {
        // We shall handle our inputs here.

        float playerVInput;

        #if UNITY_STANDALONE
                hInput = Input.GetAxis("Horizontal");
                playerVInput = Input.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
                hInput = 2.0f * mJoystick.Horizontal;
                playerVInput = 2.0f * mJoystick.Vertical;
#endif

        if (playerVInput > 0 || playerVInput < 0)
        {//when vinput is either back or forth
            vInput = Mathf.Clamp(vInput + playerVInput * inputDamper, -1.0f, 1.0f); //cap the vinput to 0 or 1f
        }
        //else add resistance to the movement
        else 
        {//when playerinput is exactly zero
            if(vInput > 0.1f || vInput < -0.1f)
            { //this is to prevent the vinput to constantly get minus or plus
                vInput = 0;
            }

            if(vInput > 0)
            {
                vInput -=  resistance * Time.deltaTime;
            }
            else if(vInput < 0)
            {
                vInput += resistance * Time.deltaTime;  
            }
        }

        speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        { //prevent players from 
            speed = mWalkSpeed * 2.0f;
        }

        if (crouch)
        {
            speed = mWalkSpeed * speedReductionWhenCrouching;
        }

        if (Input.GetKey(KeyCode.Space) && mCharacterController.isGrounded)
        {
            jump = true;
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            crouch = !crouch;
            Crouch();
        }

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) //start attacking
        {
            attack = true;
            Attack();
        }

    }

    public void AmyNextMove()
    {
        if (mAnimator == null) return;

        if(!crouch && attack)
        {
            Attack();
            attack = false;
            return;
            //start an attack then stop the entire function
        }

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
        mCharacterController.Move(mVelocity * Time.deltaTime);
    }

    void Jump()
    {
        mAnimator.SetTrigger("Jump");
        if(vInput > 0.2f || vInput < -0.2f)
        {
            StartCoroutine(ChargingUpBeforeJumping());
        }
        else
        {
            mVelocity.y += Mathf.Sqrt(mJumpHeight * -2f * mGravity);
        }
    }

    private IEnumerator ChargingUpBeforeJumping()
    {
        yield return new WaitForSeconds(2);
        mVelocity.y += Mathf.Sqrt(mJumpHeight * -2f * mGravity);
    }

    private Vector3 HalfHeight;
    private Vector3 tempHeight;
    void Crouch()
    {
        //called in the handle input
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
        if (mCharacterController.isGrounded && mVelocity.y < 0) mVelocity.y = 0f;
    }

    #region attackingFunction

    void Attack()
    {
        if (Time.time - lastClickTime >= 0.5f && comboCount <= combo.Count)
        {
            CancelInvoke(EndComboFunctionName);
            CancelInvoke("Move");
            if(Time.time - lastClickTime >= 0.2f)
            {
                mAnimator.runtimeAnimatorController = combo[comboCount].controller;
                mAnimator.Play("attack", 0, 0);

                comboCount++;
                lastClickTime = Time.time;
                if(comboCount == combo.Count ) comboCount = 0;
            }
        }
    }

    void ExitAttack()
    {
        if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke(EndComboFunctionName , 1);
        }
    }

    void EndCombo() //called through invoke
    {
        comboCount = 0;
        lastComboEnd = Time.time;
    }

    #endregion 

    /*
     * todo
     * 1. Have a acceleration system (to prevent players from stop suddenly)
     * 2. implement a FSM
     * 3. 
     */

    // Need the movement (can make the player injured and account for different camera states)
    // implement punching (cant move)
    // crouching
    // recovery
}
