using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BikeMovementController : MonoBehaviour
{
    [SerializeField] private float _accelerationPower = 10f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private List<WheelJoint2D> _wheels;

    public static bool isMovementBlocked;

    private Rigidbody2D _rb;
    private WheelJoint2D _rearWheel;
    private WheelJoint2D _frontWheel;
    private JointMotor2D _rearMotor;
    private JointMotor2D _frontMotor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        FindRearAndFrontWheels();
        InitializeMotors();
    }

    private void Update()
    {
        if (!isMovementBlocked)
        {
            HandleMotor();
            HandleBalance();
        }
    }

    private void FixedUpdate()
    {
        LimitSpeed();
    }

    private void FindRearAndFrontWheels()
    {
        _wheels.Sort((w1, w2) => w1.transform.position.x.CompareTo(w2.transform.position.x));
        _rearWheel = _wheels[0];
        _frontWheel = _wheels[1];
    }

    private void InitializeMotors()
    {
        _rearMotor = _rearWheel.motor;
        _frontMotor = _frontWheel.motor;
    }

    private void HandleMotor()
    {
        float targetSpeed = 0f;

        if (Input.GetMouseButton(0))
        {
            Vector3 tapPosition = Input.mousePosition;
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

            if (tapPosition.x > screenCenter.x)
                targetSpeed = _maxSpeed;
            else
                targetSpeed = -_maxSpeed;
        }

        ApplyTargetSpeed(targetSpeed);
    }

    private void ApplyTargetSpeed(float targetSpeed)
    {
        _rearMotor.motorSpeed = targetSpeed;
        _rearWheel.motor = _rearMotor;

        _frontMotor.motorSpeed = targetSpeed;
        _frontWheel.motor = _frontMotor;
    }

    private void LimitSpeed()
    {
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxSpeed);
    }

    private void HandleBalance()
    {
        float accelerometerInput = Input.GetAxis("Horizontal"); //Input.acceleration.x;

        if (accelerometerInput < 0)
            _rearMotor.motorSpeed -= _accelerationPower * Mathf.Abs(accelerometerInput);
        else if (accelerometerInput > 0)
            _frontMotor.motorSpeed += _accelerationPower * Mathf.Abs(accelerometerInput);
    }
}
