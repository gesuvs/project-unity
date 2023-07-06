using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    public Vector2 move;
    public Animator animator;
    public int isRunForwardHash;

    PlayerInput input;

    bool movementPressed;
    void Awake()
    {
        Debug.Log("Awake");

        input = new PlayerInput();
        input.Player.Move.performed += ctx =>
        {
            ctx.ReadValue<Vector2>();
        };
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove");

        move = context.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        Debug.Log("MovePlayer");
        Debug.Log($"IsOwner: {IsOwner}");

        // if (!IsOwner) return;
        var movement = new Vector3(move.x,0f,move.y);
        
        transform.Translate(movement * (speed * Time.deltaTime),Space.World);
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(movement), 0.15f);
        
        movementPressed = move.x != 0 || move.y != 0;
        animator.SetBool(isRunForwardHash, movementPressed);
        Debug.Log($"isRunForwardHash: {isRunForwardHash}");
        Debug.Log($"movementPressed: {movementPressed}");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        isRunForwardHash = Animator.StringToHash("isRunForward");
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void OnEnable()
    {
        input.Player.Enable();
    }
    
    void OnDisable()
    {
        input.Player.Disable();
    }
}
