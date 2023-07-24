using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BikeMovementController : MonoBehaviour
{
    [SerializeField] private float _accelerationPower = 10f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private List<WheelJoint2D> _wheels;
    [SerializeField] private Transform _bikePivotTransform;

    private Rigidbody2D _rb;
    private JointMotor2D _motor;
    private bool isReversing = false;

    private KeyCode accelerationKey = KeyCode.Space;    
    private KeyCode reverseKey = KeyCode.Backspace;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _motor = new JointMotor2D { motorSpeed = 0, maxMotorTorque = 10000 };
    }

    private void Update()
    {
        CheckInput();
        HandleMotor();
    }

    private void FixedUpdate()
    {
        LimitSpeed();
    }

    private void CheckInput()
    {
         isReversing = Input.GetKey(reverseKey);
    }

    private void HandleMotor()
    {
        float targetSpeed = 0f;
        
        if (isReversing)
        {
            targetSpeed = -_maxSpeed;
        }
        else if (Input.GetKey(accelerationKey))
        {
            targetSpeed = _maxSpeed;
        }       
        _motor.motorSpeed = Mathf.MoveTowards(_motor.motorSpeed, targetSpeed, _accelerationPower * Time.deltaTime);
       
        foreach (var wheel in _wheels)
        {
            wheel.motor = _motor;
        }        
    }

    private void LimitSpeed()
    {        
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxSpeed);
    }
}
