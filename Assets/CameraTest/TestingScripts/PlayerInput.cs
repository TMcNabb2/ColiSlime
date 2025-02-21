using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    [SerializeField]
    UIDocument _uiDocument;
    bool isPaused;
    [SerializeField] private float speed;
    private float _currentVelocity;
    [SerializeField] private float smoothTime = 0.05f;

    // References
    private Animator _animator;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        isPaused = _uiDocument.rootVisualElement.Q<VisualElement>("PauseScreen").visible = true;
        _animator.applyRootMotion = true; // Enable root motion
    }

    private void Update()
    {
        if (_input.sqrMagnitude > 0 && isPaused)
        {
            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            var angleSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angleSmooth, 0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("PauseScreen").visible = true;
            Time.timeScale = 0;
        }
        // Update animator Speed parameter for blend tree
        _animator.SetFloat("Speed", _input.sqrMagnitude > 0 ? 1f : 0, 0.1f, Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _input = ctx.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0, _input.y);
        Debug.Log($"Move: {_input}");
    }

    private void OnAnimatorMove()
    {
        if (_characterController != null)
        {
            // Move the parent GameObject using root motion
            _characterController.Move(_animator.deltaPosition);
        }
    }
}
