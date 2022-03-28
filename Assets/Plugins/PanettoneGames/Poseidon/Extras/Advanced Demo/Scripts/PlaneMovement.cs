using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneMovement : MonoBehaviour
{
    [Header("Propeller")]
    [SerializeField] private Transform propeller;
    [SerializeField] [Range(1, 200)] private float propellerSpeed = 150;

    [Header("Movement")]
    [SerializeField] InputActionReference leftStick;
    [SerializeField] InputActionReference rightStick;
    [SerializeField] [Range(1, 80)] private float moveSpeed = 20;
    [SerializeField] [Range(1, 120)] private float spinSpeed = 50f;

    float rollLimit = 30.0f;
    float pitchLimit = 40.0f;

    private Vector2 movement;
    private Vector2 look;

    private void OnEnable()
    {
        leftStick.asset.Enable();
        rightStick.asset.Enable();
    }

    private void OnDisable()
    {
        leftStick.asset.Disable();
        rightStick.asset.Disable();
    }

    private void FixedUpdate()
    {
        if (leftStick == null || rightStick == null) return;

        //Movement & Camera
        movement = leftStick.action.ReadValue<Vector2>();
        look = rightStick.action.ReadValue<Vector2>();

        //Propeller
        propeller.Rotate(Vector3.forward * propellerSpeed);

        //Rotate
        if (look != Vector2.zero)
            transform.Rotate(look.y * spinSpeed, 0, -look.x * spinSpeed);

        //Move
        if (movement != Vector2.zero)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            if (look == Vector2.zero)
            {
                float roll = movement.x * rollLimit;
                float pitch = movement.y * pitchLimit;

                Quaternion target = Quaternion.Euler(-pitch, 0, -roll);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, spinSpeed * Time.deltaTime);
            }
        }

        //Reset Rotation
        if (look == Vector2.zero && movement == Vector2.zero)
        {
            Quaternion target = Quaternion.identity;
            transform.rotation = Quaternion.Slerp(transform.rotation, target, spinSpeed * Time.deltaTime);
        }
    }

}
