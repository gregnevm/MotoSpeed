using System;
using System.Collections.Generic;
using UnityEngine;

public class BikeMovementController : MonoBehaviour
{   
    [SerializeField] private float _motorSpeed;

    [SerializeField] private WheelJoint2D _backWheel;
    [SerializeField] private WheelJoint2D _forwardWheel;

    private float _forwardWheelMultiplier = 1f;
    private float _backWheelMultiplier = 1f;
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Balance();
    }
    private void Move()
    {
        float input = 0f;
        if (Input.GetMouseButton(0))
        {
            Vector3 tapPosition = Input.mousePosition;
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

            input = tapPosition.x > screenCenter.x ? 1 : -1;
        }

        if (input != 0)
        {
            JointMotor2D motor = new JointMotor2D()
            {
                motorSpeed = _motorSpeed * input * _backWheelMultiplier,
                maxMotorTorque = 10000
            };

            _backWheel.motor = motor;

            motor = new JointMotor2D()
            {
                motorSpeed = _motorSpeed * input * _forwardWheelMultiplier,
                maxMotorTorque = 10000
            };

            motor.motorSpeed *= _forwardWheelMultiplier;
            _forwardWheel.motor = motor;
        }
        else
        {
            _backWheel.useMotor = false;
            _forwardWheel.useMotor = false;
        }
    }
    private void Balance()
    {
        float input = Input.acceleration.x;
#if UNITY_EDITOR
        input = Input.GetAxis("Horizontal");
#endif
        if (input < 0)
        {
            _backWheelMultiplier = 1.5f*-input;
            _forwardWheelMultiplier = 0.75f*-input;
        }
        else if (input > 0)
        {
            _backWheelMultiplier = 0.75f*input;
            _forwardWheelMultiplier = 1.5f*input;
        }
        else
        {
            _backWheelMultiplier = 1;
            _forwardWheelMultiplier = 1;
        }
    }
}
