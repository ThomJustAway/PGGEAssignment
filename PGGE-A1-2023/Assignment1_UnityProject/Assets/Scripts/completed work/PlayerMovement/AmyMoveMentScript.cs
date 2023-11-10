using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AmyMoveMentScript : MonoBehaviour
{
    //add values here to be set to the movementstateManager
    public static AmyMoveMentScript instance { get; private set; }
    private MovementStateManager m_StateManager;
    public PlayerInput PlayerInput { get; private set; }

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
    #endregion

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            print("Amy movement script can only exist in one at a time");
            Destroy(instance); 
        }
        PlayerInput = GetComponent<PlayerInput>();
        m_StateManager = new MovementStateManager();
    }

    #region inputsystem
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    private void OnJump()
    {
        canJump = true;
    }

    #endregion

}
