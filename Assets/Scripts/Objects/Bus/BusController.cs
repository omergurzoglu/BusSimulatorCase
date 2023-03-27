using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Bus
{
    /// <summary>
    /// Gets the player input with the new input system, controls the bus with wheelColliders
    /// </summary>
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
        [SerializeField] private Transform busLeftDoor;
        [SerializeField] private Transform busRightDoor;
        
        [Header("Variables")]
        [SerializeField] private float accelerationForce;
        [SerializeField] private float brakeForce;
        [SerializeField] private float maxSteerAngle=15f;
        
        private float _currentSteerAngle;
        private float _targetSteerAngle;
        private const float SteerSpeed = 5f;
        
        private bool _acceleratePressed;
        private bool _brakePressed;
        private bool _reversePressed;
        private bool _forceStopBus;
        
        [SerializeField]private PlayerInput playerInput;
        public static event Action<Transform> BroadCastBusEntryPosForPassengers;
        #endregion

        #region MonoBehavior
        private void FixedUpdate()
        {
            CheckIfTilted();
            AdjustWheelForce();
            SmoothSteer();
            ForceStopBus();
        }
        private void Update() => MatchMeshRotation();

        #endregion
        #region MainMethods
        private void SmoothSteer()
        {
            //To smoothly turn the steering, lerp between currentAngle and targetAngle every frame
            _currentSteerAngle = Mathf.Lerp(_currentSteerAngle, _targetSteerAngle, Time.fixedDeltaTime * SteerSpeed);

            //Apply steering
            foreach (var wheel in frontWheelColliders)
            {
                wheel.steerAngle = _currentSteerAngle;
            }
        }
        private void AdjustWheelForce()
        {
            //Set multipliers depending on the keys state
            float accelerationCheck = (_acceleratePressed || _reversePressed) ? 1f : 0f;
            float direction = _reversePressed ? -1f : 1f;
            
            //Apply motor and brake torque to corresponding wheels 
            foreach (var wheel in frontWheelColliders)
            {
                //forward or backward depending on direction multiplier
                wheel.motorTorque = accelerationForce * accelerationCheck * direction;
                
            }
            float decelerationCheck = _brakePressed ? 1f : 0f;
            foreach (var wheel in allWheelColliders)
            {
                wheel.brakeTorque=  brakeForce * decelerationCheck;
            }
        }
        private void CheckIfTilted()
        {
            //Check every frame if bus is tilted, if so fix rotation
            if(Vector3.Dot(transform.up, Vector3.down) > 0)
            {
                transform.position += new Vector3(0, 5, 0);
                Quaternion currentRotation = transform.rotation;
                Quaternion newRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);
                transform.rotation = newRotation;
            }
        }
        private void ForceStopBus()
        {
            //If the bool is true , stop the bus
            if (!_forceStopBus) return;
            foreach (var wheel in allWheelColliders)
            {
                wheel.brakeTorque = 4000f;
            }
        }
        public void LockBrake(bool lockBus)
        {
            //To switch between bus states
            if (lockBus)
            {
                playerInput.enabled = false;
                _forceStopBus = true;
            }
            else 
            {
                _forceStopBus = false;
                playerInput.enabled = true;
            }
        }
        private void AdjustWheelMeshRotation(WheelCollider wheelCollider, Transform wheelVisualTransform)
        {
            //Match the wheel visuals with wheelColliders position and rotation
            
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
        public void SetDoorState(bool isOpen)
        {
            //Open or close doors
            if (isOpen)
            {
                busLeftDoor.DOLocalRotate(new Vector3(0, -90, 0), 0.6f).SetEase(Ease.InBounce);
                busRightDoor.DOLocalRotate(new Vector3(0, 90, 0), 0.6f).SetEase(Ease.InBounce);
            }
            else
            {
                busLeftDoor.DOLocalRotate(new Vector3(0, 0, 0), 0.6f).SetEase(Ease.InBounce);
                busRightDoor.DOLocalRotate(new Vector3(0, 0, 0), 0.6f).SetEase(Ease.InBounce);
            }
        }
        private static void OnBroadCastBusEntryPosForPassengers(Transform obj) => BroadCastBusEntryPosForPassengers?.Invoke(obj);
        
        //Send door transform to the subscribers, in this case , new spawned passengers
        public void SendDoorTransformToPassengers() => OnBroadCastBusEntryPosForPassengers(busLeftDoor);
        private IEnumerator BusTakeOffCoroutine()
        {
            //Wait 5 second before getting the next schedule,5 seconds is enough for the passengers to load the bus 
            yield return new WaitForSeconds(5f);
            LockBrake(false);
            SetDoorState(false);
            LogisticManager.Instance.DesignateNewSchedule();
        }
        public void BusTakeOff() => StartCoroutine(BusTakeOffCoroutine());
        #endregion
        
        /// <summary>
        /// New input system is linked through unity events CallbackContext
        /// WASD is the direction keys , SPACE for brakes
        /// The methods check when the key is pressed and released and set the context bool accordingly 
        /// </summary>
        #region Input
        
        public void OnAccelerate(InputAction.CallbackContext context)
        {
            ContextCheckForAcceleration(context,ref _acceleratePressed);
        }
        public void OnDecelerate(InputAction.CallbackContext context)
        {
            ContextCheckForAcceleration(context, ref _brakePressed);
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
