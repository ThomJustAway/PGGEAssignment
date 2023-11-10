using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    #region inputValues
        public Vector2 inputVector { get; private set; }
        public bool canJump { get; private set; }
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

    #region misc values

        [SerializeField] private float turnRate;
        public float TurnRate { get {  return turnRate; } }

        [SerializeField] private float rotationSpeed;
        public float RotationSpeed { get { return rotationSpeed; } }
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

    #region inputsystem
    //all of this function are invoke as callbacks
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    private void OnJump()
    {
        canJump = true;
    }

    private void OnSprint()
    {
        Sprinting = !Sprinting;
    }

    #endregion

}
