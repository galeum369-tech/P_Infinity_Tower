using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    [Header("인풋 액션 설정 (인스펙터에서 키 바인딩)")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction fallAction;
    [SerializeField] private InputAction interactAction;

    public Vector2 MoveInput { get; private set; }
    public event Action OnJumpPressed;
    public event Action OnFallPressed;
    public event Action OnInteractPressed;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        fallAction.Enable();
        interactAction.Enable();

        jumpAction.performed += ctx => OnJumpPressed?.Invoke();
        fallAction.performed += ctx => OnFallPressed?.Invoke();
        interactAction.performed += ctx => OnInteractPressed?.Invoke();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        fallAction.Disable();
        interactAction.Disable();
    }

    void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
    }
}