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
    
    public Animator Animator { get { return animator; } }
    [SerializeField] private Animator animator; 
    public CharacterController CharacterController { get; private set; }

    //did getters and setter to make sure that other scripts can access the values without changing them.
    #region inputValues
        public Vector2 InputVector { get; private set; } //values of the players input
        public bool CanJump { get; private set; } //see if the frame allows the player to jump
        public void StopJump() { CanJump = false; } //a function to stop the jump bool after being called
        public bool CanCrouch { get; private set; } //see if a frame allows the player to crouch
    #endregion

    #region values for moving amy
        [SerializeField] private float walkSpeed; //what is a speed mulitplier of the player
        public float WalkSpeed { get { return walkSpeed; } }

        [SerializeField] public float inputDamper; //I cant figure out the name for this one but it essentially 
        //reduce animation movement at a set period of time if the player suddenly stops moving
        public float InputDamper { get { return inputDamper; } }

        public bool Sprinting { get; private set; } //see if the player can sprint in a frme
    #endregion

    #region values for jumping Amy

    [SerializeField] private float jumpHeight; //what is the jump height for the player
    public float JumpHeight { get { return jumpHeight; } }

    #endregion

    #region values for crouching Amy

    [Range(0,1)]
    [SerializeField] private float crouchingSpeedReduction; //this reduce the speed when the player is crouching
    public float CrouchingSpeedReduction { get {  return crouchingSpeedReduction; } }
    #endregion

    #region values for reloading amy
        public bool IsReloading { get; private set; } //see if the frame allow amy to reload
        public void FinishReloading() { IsReloading = false; } //once the reload animation has been complete 
    //this function will be called to tell that it has finish reloading
    #endregion

    #region values for Amy punching
    public bool IsAttacking { get; private set; } //if the player can attack
    public void AttackingDone() { IsAttacking = false; } 
    //this is called to either prevent premature calling of the attack or if the attack animation has been completed
    [SerializeField] private List<AttackSO> combo = new List<AttackSO>(); //list of attack combo the player can do
    public List<AttackSO> Combo { get {  return combo; } }
    #endregion

    #region idle animation
    [SerializeField] private List<AnimatorOverrideController> idleAnimations; //list of idle animation the player can do
    public List<AnimatorOverrideController> IdleAnimations { get { return idleAnimations; } }
    #endregion

    #region misc values
    public bool IsMoving { get { return InputVector != Vector2.zero; } } //simple check to see if the player is moving

        [SerializeField] private float turnRate; //for player to rotate around, for camera type like follow rot_pos
        public float TurnRate { get {  return turnRate; } }

        [SerializeField] private float rotationSpeed;
        public float RotationSpeed { get { return rotationSpeed; } }

        [SerializeField] private float gravity; //a constant that pulls the player down
        public float Gravity { get { return gravity; } }

        private Vector3 velocity; //a vector to be used to pull the player down
        public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
            
        public bool isGrounded { get { return CharacterController.isGrounded; } } //check if the player is grounded
    #endregion

    private void Start()
    {
        //init component and setting up the singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            print("Amy movement script can only exist in one at a time");
            Destroy(Instance); 
        }
        //setting value
        CharacterController = GetComponent<CharacterController>();
        movementStateManager = new MovementStateManager(); //setting up state manager to start calling it
    }

    private void Update()
    {
        movementStateManager.HandleState();
    }

    private void FixedUpdate()
    {
        movementStateManager.HandleFixedUpdateState();
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
