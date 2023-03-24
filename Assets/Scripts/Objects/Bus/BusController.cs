using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Bus
{
    public class BusController : MonoBehaviour
    {
        #region Fields
        
        [Header("Wheel References")]
        [SerializeField] private WheelCollider[] allWheelColliders=new WheelCollider[4];
        [SerializeField] private WheelCollider[] frontWheelColliders=new WheelCollider[2];
        [SerializeField] private WheelCollider frontLeftWheel;
        [SerializeField] private WheelCollider frontRightWheel;
        [SerializeField] private WheelCollider backLeftWheel;
        [SerializeField] private WheelCollider backRightWheel;
        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheelTransform;
        [SerializeField] private Transform backLeftWheelTransform;
        [SerializeField] private Transform backRightWheelTransform;
        
        [Header("Variables")]
        [SerializeField] private float accelerationForce;
        [SerializeField] private float decelerationForce;
        [SerializeField] private float maxSteerAngle=15f;
        
        private float _currentSteerAngle;
        private float _targetSteerAngle;
        private const float SteerSpeed = 5f;
        
        private bool _acceleratePressed;
        private bool _deceleratePressed;
        private bool _reversePressed;
        public bool inBusStop;
        
        #endregion

        #region MonoBehavior

        private void FixedUpdate()
        {
            AdjustWheelForce();
            SmoothSteer();
            MatchMeshRotation();
        }
        #endregion
        
        #region MainMethods

        private void SmoothSteer()
        {
            _currentSteerAngle = Mathf.Lerp(_currentSteerAngle, _targetSteerAngle, Time.fixedDeltaTime * SteerSpeed);

            foreach (var wheel in frontWheelColliders)
            {
                wheel.steerAngle = _currentSteerAngle;
            }
        }
        
        private void AdjustWheelForce()
        {
            float accelerationCheck = (_acceleratePressed || _reversePressed) ? 1f : 0f;
            float direction = _reversePressed ? -1f : 1f;
            foreach (var wheel in frontWheelColliders)
            {
                wheel.motorTorque = accelerationForce*accelerationCheck*direction;
                
            }
            float decelerationCheck = _deceleratePressed ? 1f : 0f;
            foreach (var wheel in allWheelColliders)
            {
                wheel.brakeTorque=  decelerationForce*decelerationCheck;
            }
        }

        private void AdjustWheelMeshRotation(WheelCollider wheelCollider, Transform wheelVisualTransform)
        {
            wheelCollider.GetWorldPose(out Vector3 pos,out Quaternion rot);
            wheelVisualTransform.position = pos;
            wheelVisualTransform.rotation = rot;
        }

        private void MatchMeshRotation()
        {
            AdjustWheelMeshRotation(frontRightWheel,frontRightWheelTransform);
            AdjustWheelMeshRotation(frontLeftWheel,frontLeftWheelTransform);
            AdjustWheelMeshRotation(backLeftWheel,backLeftWheelTransform);
            AdjustWheelMeshRotation(backRightWheel,backRightWheelTransform);
        }
        
        #endregion

        #region Input
        
        public void OnAccelerate(InputAction.CallbackContext context)
        {
            ContextCheckForAcceleration(context,ref _acceleratePressed);
        }

        public void OnDecelerate(InputAction.CallbackContext context)
        {
            ContextCheckForAcceleration(context, ref _deceleratePressed);
        }

        public void OnSteerLeft(InputAction.CallbackContext context)
        {
            
            ContextCheckForSteer(context,-1);
            
        }
        public void OnSteerRight(InputAction.CallbackContext context)
        {
            ContextCheckForSteer(context, 1);
            
        }

        public void OnReverse(InputAction.CallbackContext context)
        {
            ContextCheckForAcceleration(context,ref _reversePressed);
        }
        
        private void ContextCheckForAcceleration(InputAction.CallbackContext context, ref bool contextBool)
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

        private void ContextCheckForSteer(InputAction.CallbackContext context, int direction)
        {
            
            if (context.performed)
            {
                _targetSteerAngle = maxSteerAngle * direction;
            }
            else if (context.canceled)
            {
                _targetSteerAngle = 0;
            }
            
        }
        #endregion
        
        
    }
    
    
}
