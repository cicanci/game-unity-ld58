using UnityEngine;
using UnityEngine.InputSystem; // new Input System

namespace Cicanci
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;

        private Rigidbody rb;
        private Vector2 moveInput;
        private PlayerControls controls;

        void Awake()
        {
            controls = new PlayerControls();
        }

        void OnEnable()
        {
            controls.Player.Enable();
            controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        }

        void OnDisable()
        {
            controls.Player.Disable();
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // Convert 2D input (x, y) into 3D movement (x, z)
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        }
    }
}