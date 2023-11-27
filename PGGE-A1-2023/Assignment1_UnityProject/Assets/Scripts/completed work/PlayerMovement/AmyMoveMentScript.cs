using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class AmyMoveMentScript : MonoBehaviour
{
    //add values here to be set to the movementstateManager
    public static AmyMoveMentScript Instance { get; private set; }
    private MovementStateManager movementStateManager;
    private PlayerInput PlayerInput; //probably can delete this
    
    public Animator Animator { get { return animator; } }
    [SerializeField] private Animator animator; 
    public CharacterController CharacterController { get; private set; }

    //did getters and setter to make sure that other scripts can access the values without changing them.
    #region inputValues
        public Vector2 InputVector { get; private set; }
        public bool CanJump { get; private set; }
        public void StopJump() { CanJump = false; }
        public bool CanCrouch { get; private set; }
    #endregion

    #region values for moving amy
        [SerializeField] private float walkSpeed;
        public float WalkSpeed { get { return walkSpeed; } }

        [SerializeField] public float inputDamper;
        public float InputDamper { get { return inputDamper; } }
        [SerializeField] public float resistance { get; private set; }
        public float Resistance { get { return resistance; } }

        public bool Sprinting { get; private set; }
    #endregion

    #region values for jumping Amy

    [SerializeField] private float jumpHeight;
    public float JumpHeight { get { return jumpHeight; } }

    #endregion

    #region values for crouching Amy

    [Range(0,1)]
    [SerializeField] private float crouchingSpeedReduction;
    public float CrouchingSpeedReduction { get {  return crouchingSpeedReduction; } }
    #endregion

    #region values for reloading amy
        public bool IsReloading { get; private set; }
        public void FinishReloading() { IsReloading = false; }
    #endregion

    #region values for Amy punching

    public bool IsAttacking { get; private set; }
    public void AttackingDone() { IsAttacking = false; }
    [SerializeField] private List<AttackSO> combo = new List<AttackSO>();
    public List<AttackSO> Combo { get {  return combo; } }
    #endregion

    #region idle animation
    [SerializeField] private List<AnimatorOverrideController> idleAnimations;
    public List<AnimatorOverrideController> IdleAnimations { get { return idleAnimations; } }
    #endregion

    #region misc values

    public bool IsMoving { get { return InputVector != Vector2.zero; } }

        [SerializeField] private float turnRate;
        public float TurnRate { get {  return turnRate; } }

        [SerializeField] private float rotationSpeed;
        public float RotationSpeed { get { return rotationSpeed; } }

        [SerializeField] private float gravity;
        public float Gravity { get { return gravity; } }

        private Vector3 velocity;
        public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
            
        public bool isGrounded { get { return CharacterController.isGrounded; } }
    #endregion

    private void Start()
    {
        //init component
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            print("Amy movement script can only exist in one at a time");
            Destroy(Instance); 
        }
        PlayerInput = GetComponent<PlayerInput>();
        CharacterController = GetComponent<CharacterController>();
        movementStateManager = new MovementStateManager();
    }

    private void Update()
    {
        movementStateManager.Move();
    }

    private void FixedUpdate()
    {
    
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        // apply gravity.
        velocity.y += gravity * Time.deltaTime;
        if (CharacterController.isGrounded && velocity.y < 0) velocity.y = gravity * 0.01f;
        //because of some bug with the character controller it wont work if velocity is
        //set to 0 https://stackoverflow.com/questions/39732254/isgrounded-in-charactercontroller-not-stable
    }

    #region inputsystem
    //all of this function are invoke as callbacks
    private void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }

    private void OnJump()
    {
        
        if (!CanCrouch && CharacterController.isGrounded)
        {
            CanJump = true;
        }
    }

    private void OnSprint()
    {
        Sprinting = !Sprinting;
    }

    private void OnCrouching()
    {
        CanCrouch = !CanCrouch;
    }

    private void OnAttack()
    {
        if (!CanCrouch)
        {
            IsAttacking = true;
        }
    }

    private void OnReload()
    {
        if(!CanCrouch && !CanJump && !IsAttacking)
        {
            IsReloading = true;
        }
    }
    #endregion

}
