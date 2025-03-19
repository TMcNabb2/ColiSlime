using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [Header("Movement Settings")]
    public float movementSpeed;
    public float dashForce;
    public float dashCooldown;
    
    
    [Header("Input Settings")]
    public InputActionReference MoveAction;
    public InputActionReference DashAction;
    public InputActionReference PauseAction;
    [Header("Debug Settings/Values")]
    private Rigidbody _rigidBody;
    private Vector3 _inputDirection;
    
    [SerializeField]
    private float _dashCooldownTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        MoveAction.action.started +=  MoveInput;
        MoveAction.action.canceled +=  MoveInput;
        MoveAction.action.performed +=  MoveInput;
        DashAction.action.performed +=  DashInput;
        PauseAction.action.performed += PauseInput;



    }

    private void OnDisable()
    {
        MoveAction.action.started -= MoveInput;
        MoveAction.action.canceled -= MoveInput;
        MoveAction.action.performed -= MoveInput;
        DashAction.action.performed -= DashInput;
        PauseAction.action.performed -= PauseInput;
    }

    private void MoveInput(InputAction.CallbackContext ctx)
    {
        _inputDirection = ctx.ReadValue<Vector2>();
    }

    private void DashInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            Dash();
        }
    }
    private void PauseInput(InputAction.CallbackContext ctx)
    {
        UIDocument _uiDocument = FindFirstObjectByType<UIDocument>();
        _uiDocument.rootVisualElement.Q<VisualElement>("PauseScreen").visible = true;
        Time.timeScale = 0;

    }

    void Dash()
    {
        if (_dashCooldownTimer <= 0f)
        {
            if (_inputDirection != Vector3.zero)
            {
                _rigidBody.AddForce(new Vector3(_inputDirection.x,0,_inputDirection.y) * dashForce,ForceMode.Impulse);
            }
            else
            {
                _rigidBody.AddForce(new Vector3(0,0,1) * dashForce,ForceMode.Impulse);
            }
            _dashCooldownTimer = dashCooldown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        _rigidBody.AddForce(new Vector3(_inputDirection.x,0,_inputDirection.y) * movementSpeed);
    }


   

   

   
}
