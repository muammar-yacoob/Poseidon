using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneMovement : MonoBehaviour
{
    [Header("Propeller")]
    [SerializeField] private Transform propeller;
    [SerializeField] [Range(1, 200)] private float propellerSpeed = 150;

    [Header("Movement")]
    [SerializeField] [Range(1, 80)] private float moveSpeed = 20;
    [SerializeField] [Range(1, 120)] private float spinSpeed = 50f;

    float rollLimit = 30.0f;
    float pitchLimit = 40.0f;

    private GameControls gameControls;
    private InputAction movement;
    private InputAction look;

    private void OnEnable()
    {
        gameControls = new GameControls();

        movement = gameControls.Player.Movement;
        movement.Enable();

        look = gameControls.Player.Look;
        look.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        look.Disable();
    }

    private void FixedUpdate()
    {

        var move = movement.ReadValue<Vector2>();
        var spin = look.ReadValue<Vector2>();

        //Propeller
        propeller.Rotate(Vector3.forward * propellerSpeed);

        //Rotate
        if (spin != Vector2.zero)
            transform.Rotate(spin.y * spinSpeed, 0, -spin.x * spinSpeed);

        //Move
        if (move != Vector2.zero)
        {
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            if (spin == Vector2.zero)
            {
                float roll = move.x * rollLimit;
                float pitch = move.y * pitchLimit;

                Quaternion target = Quaternion.Euler(-pitch, 0, -roll);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, spinSpeed * Time.deltaTime);
            }
        }

        //Reset Rotation
        if (spin == Vector2.zero && move == Vector2.zero)
        {
            Quaternion target = Quaternion.identity;
            transform.rotation = Quaternion.Slerp(transform.rotation, target, spinSpeed * Time.deltaTime);
        }
    }

}
