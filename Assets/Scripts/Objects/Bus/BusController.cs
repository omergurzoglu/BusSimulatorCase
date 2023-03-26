using System;
using System.Collections;
using DG.Tweening;
using Managers;
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
        [SerializeField] private Transform busLeftDoor;
        [SerializeField] private Transform busRightDoor;
        
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
        private void CheckIfTilted()
        {
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
            if (!_forceStopBus) return;
            foreach (var wheel in allWheelColliders)
            {
                wheel.brakeTorque = 4000f;
            }
        }
        public void LockBrake(bool lockBus)
        {
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
        public void SendDoorTransformToPassengers() => OnBroadCastBusEntryPosForPassengers(busLeftDoor);
        private IEnumerator BusTakeOffCoroutine()
        {
            yield return new WaitForSeconds(5f);
            LockBrake(false);
            SetDoorState(false);
            LogisticManager.Instance.DesignateNewSchedule();
        }
        public void BusTakeOff() => StartCoroutine(BusTakeOffCoroutine());
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
