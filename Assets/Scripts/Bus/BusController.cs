
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bus
{
    public class BusController : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private WheelCollider[] _wheelColliders;
        [SerializeField] private WheelCollider frontLeftWheel;
        [SerializeField] private WheelCollider frontRightWheel;
        [SerializeField] private WheelCollider backLeftWheel;
        [SerializeField] private WheelCollider backRightWheel;
        [SerializeField] private float accelerationForce;
        [SerializeField] private float decelerationForce;
        [SerializeField] private bool _acceleratePressed = false;
        [SerializeField] private bool _deceleratePressed = false;
        
        #endregion

        #region MonoBehavior

        private void Awake()
        {
            RegisterWheels();
        }

        private void FixedUpdate()
        {
            AdjustWheelForce();
        }
        #endregion
        
        #region MainMethods

        private void RegisterWheels()
        { 
            _wheelColliders = GetComponentsInChildren<WheelCollider>();
        }
        
        private void AdjustWheelForce()
        {
            float accelerationCheck = _acceleratePressed ? 1f : 0f;
            float decelerationCheck = _deceleratePressed ? 1f : 0f;

            frontLeftWheel.motorTorque = accelerationCheck * accelerationForce;
            frontRightWheel.motorTorque = accelerationCheck * accelerationForce;
            backLeftWheel.motorTorque=accelerationCheck * accelerationForce;
            backRightWheel.motorTorque=accelerationCheck * accelerationForce;
            
            
            frontLeftWheel.brakeTorque = decelerationCheck * decelerationForce;
            frontRightWheel.brakeTorque = decelerationCheck * decelerationForce;
            backLeftWheel.brakeTorque=decelerationCheck * decelerationForce;
            backRightWheel.brakeTorque=decelerationCheck * decelerationForce;
        }

        #endregion

        #region Input
        
        public void OnAccelerate(InputAction.CallbackContext context)
        {
            ContextCheck(context,ref _acceleratePressed);
        }

        public void OnDecelerate(InputAction.CallbackContext context)
        {
            ContextCheck(context, ref _deceleratePressed);
        }

        private void ContextCheck(InputAction.CallbackContext context, ref bool contextBool)
        {
            if (context.performed)
            {
                contextBool = true;
            }
            else if (context.canceled)
            {
                contextBool = false;
            }
            
        }
        
        #endregion
        
        
    }
    
    
}
