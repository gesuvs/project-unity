using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Vector2 move;
    public Animator animator;
    public int isRunForwardHash;

    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;
    void Awake()
    {
        input = new PlayerInput();
        input.Player.Move.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
        };
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void MovePlayer()
    { 
        var movement = new Vector3(move.x,0f,move.y);
        
        transform.Translate(movement * (speed * Time.deltaTime),Space.World);
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(movement), 0.15f);
        
        movementPressed = move.x != 0 || move.y != 0;
        animator.SetBool(isRunForwardHash, movementPressed);
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
