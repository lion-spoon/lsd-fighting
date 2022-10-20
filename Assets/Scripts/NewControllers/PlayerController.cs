using System;
using UnityEngine;
using Lib.Input;

namespace Game.Player
{
    [Serializable]
    public enum PlayerLookMode
    {
        OnlyMoving,
        AlwaysChange
    }

    [Serializable]
    public enum PlayerDirectionMode
    {
        OnlyForward,
        AnyDirection
    }

    [Serializable]
    public enum PlayerAxisMode
    {
        ByCameraAxis,
        ByLastCameraAxis
    }

    public struct DirectionAxis
    {
        public Vector3 forward;
        public Vector3 right;
    }

    public class PlayerController : MonoBehaviour
    {
        [Header("REFERENCES")]
        public InputInterface input;
        public new Camera camera;
        public Animator animator;

        [Header("SETTINGS")]
        public PlayerAxisMode axisMode = PlayerAxisMode.ByCameraAxis;
        public PlayerDirectionMode directionMode = PlayerDirectionMode.OnlyForward;
        public PlayerLookMode lookMode = PlayerLookMode.OnlyMoving;
        public float walkSpeed = 3f;
        public float runSpeed = 5f;
        public float gravityForce = 25f;
        public float jumpHeight = 5f;
        [Range(5f,50f)]
        public float movementLerpSpeed = 10f;
        [Range(5f,50f)]
        public float rotationSpeed = 10f;
        public Vector3 cameraOffset = Vector3.zero;

        [Header("STATE")]
        public Vector3 velocity;
        public Vector3 movement;
        public bool isGrounded => _controller.isGrounded;
        
        private CharacterController _controller;
        private DirectionAxis _lastDirectionAxis;

        public void TeleportTo(Vector3 target ) => _controller.Move(target - transform.position);
        public void TeleportCity() => TeleportTo(new Vector3(511.0f,51.169f,844.0f));
        public void TeleportCity2() => TeleportTo(new Vector3(692.5f,27.19f,482.9f));
        public void TeleportCity3() => TeleportTo(new Vector3(884.0f,51.0f,333.0f));

        private bool activeForceField;

        private void Start() 
        {
            _controller = GetComponent<CharacterController>();
        }
        
        
        private void OnEnable() 
        {
            _lastDirectionAxis = GetNewDirectionAxis();
        }

        public Vector3 GetCameraOffset()
        {
            return cameraOffset;
        }

        public DirectionAxis GetNewDirectionAxis()
        {
            DirectionAxis axis = new DirectionAxis();
            axis.forward = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
            axis.right = Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;
            return axis;
        }

        public DirectionAxis GetMovementDirectionAxis(bool isMovingNow)
        {
            if(!isMovingNow || axisMode == PlayerAxisMode.ByCameraAxis)
                _lastDirectionAxis = GetNewDirectionAxis();

            return _lastDirectionAxis;
        }

        private void Update() 
        {
            ativaCampoForca();

            DesativaCampoForca();
       
            
            float moveV = input["move_v"][InputInterfaceEntryMode.Raw];
            float moveH = input["move_h"][InputInterfaceEntryMode.Raw];
            bool isMoving = Mathf.Abs(moveV) > 0.01f || Mathf.Abs(moveH) > 0.01f;
            bool isRunning = input["running"];

            

            DirectionAxis axis = GetMovementDirectionAxis(isMoving);
            Vector3 newMovement = (moveV * axis.forward + moveH * axis.right).normalized; 
            movement = Vector3.Lerp(movement, newMovement, Time.deltaTime * movementLerpSpeed);

            animator.SetFloat("Velocity",newMovement.magnitude);

            float walkSpeed = isRunning ? this.runSpeed : this.walkSpeed;
            float angleDifference = 0;
            if(lookMode == PlayerLookMode.AlwaysChange || isMoving)
            {
                if(isMoving)
                {
                    Vector3 targetForward = directionMode == PlayerDirectionMode.OnlyForward ? axis.forward : movement;
                    //this.transform.forward = Vector3.Lerp(this.transform.forward, targetForward, Time.deltaTime * rotationSpeed); 
                    this.transform.forward = Vector3.RotateTowards(this.transform.forward, targetForward, Time.deltaTime * rotationSpeed, 0f);
                    angleDifference = Vector3.Angle(this.transform.forward,targetForward);
                    
                }
                else
                {
                    Vector3 targetForward = GetNewDirectionAxis().forward;
                    //this.transform.forward = Vector3.Lerp(this.transform.forward, targetForward, Time.deltaTime * rotationSpeed);
                    this.transform.forward = Vector3.RotateTowards(this.transform.forward, targetForward, Time.deltaTime * rotationSpeed, 0f);
                    angleDifference = Vector3.Angle(this.transform.forward,targetForward);
                }
            }

            walkSpeed *= angleDifference < 45f ? 1f : (1f - angleDifference / 180f);

            if(_controller.isGrounded)
            {
                velocity.y = -0.1f;
            }
            else
                velocity.y -= gravityForce * Time.deltaTime;

            if(this.input["jump"] && _controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(2f * gravityForce * jumpHeight);
            }

            _controller.Move((velocity + transform.forward * walkSpeed * movement.magnitude) * Time.deltaTime);
        }

        public void ativaCampoForca(){
            if(Input.GetKeyDown(KeyCode.R)){
                //ForceFieldController.Instance.SetOpenCloseValue(1.5f);
                GameObject.FindWithTag("Force").GetComponent<ForceFieldController>().openCloseProgress = 2.0f;
                activeForceField = true;
            }
                
        }

        public void DesativaCampoForca(){
            if(Input.GetKeyDown(KeyCode.T)){
                //ForceFieldController.Instance.SetOpenCloseValue(-1.5f);
                GameObject.FindWithTag("Force").GetComponent<ForceFieldController>().openCloseProgress = -1.0f;
                activeForceField = false;
            }  
        }
    }
}