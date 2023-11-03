using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //    private CharacterController controller;
    //private Vector3 playerVelocity;
    //    private bool groundedPlayer;
    //    [SerializeField] private Animator animator;
    //    [SerializeField]private float jumpHeight = 1.0f;
    //    [SerializeField]private float playerSpeed = 2.0f;
    //    [SerializeField] private float gravityValue = -9.81f;

    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;
    public float mWalkSpeed = 1.0f;
    public float mRotationSpeed = 50.0f;
    public bool mFollowCameraForward = true;
    public float mTurnRate = 1.0f;
    #if UNITY_ANDROID
    public FixedJoystick mJoystick;
    #endif

    private void Start()
    {
        mCharacterController = gameObject.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        SolutionMovement();
    }

    private void SolutionMovement()
    {

        #if UNITY_STANDALONE
                    float hInput = Input.GetAxis("Horizontal");
                    float vInput = Input.GetAxis("Vertical");
        #endif

        #if UNITY_ANDROID
                float hInput = 2.0f * mJoystick.Horizontal;
                float vInput = 2.0f * mJoystick.Vertical;
        #endif

        float speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mWalkSpeed * 2.0f;
        }
        if (mAnimator == null) return;

        if (mFollowCameraForward)
        {
            print("following camera");
            // rotate Player towards the camera forward.
            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0.0f, eu.y, 0.0f),
            mTurnRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime,
            0.0f);
        }


        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
        forward.y = 0.0f;
        mCharacterController.Move(forward * vInput * speed *
        Time.deltaTime);
        mAnimator.SetFloat("PosX", 0);
        mAnimator.SetFloat("PosZ", vInput * speed / 2.0f * mWalkSpeed);
    }
}
