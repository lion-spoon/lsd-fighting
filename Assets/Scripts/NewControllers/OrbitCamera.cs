using UnityEngine;
using Lib.Input;

namespace Game.Player
{
    [RequireComponent(typeof(Camera))]
    public class OrbitCamera : MonoBehaviour 
    {
        public static OrbitCamera Instance;
        public InputInterface input;

        [Range(1f, 20f)]
        public float minDistance = 2f;

        [Range(1f, 20f)]
        public float maxDistance = 5f;

        [Range(1f, 20f)]
        public float currentDistance = 5f;

        [Range(-10f,10f)]
        public float mouseSensitivityX = 4f;

        [Range(-10f,10f)]
        public float mouseSensitivityY = -4f;
        
        [Range(0.5f,10f)]
        public float mouseScrollSensitivity = 2f;   
        
        [SerializeField]
        public PlayerController player;

        [SerializeField, Min(0f)]
        float focusRadius = 5f;

        [SerializeField, Range(0f, 1f)]
        public float focusCentering = 0.5f;

        [SerializeField, Range(1f, 360f)]
        float rotationSpeed = 90f;

        [SerializeField, Range(-89f, 89f)]
        float minVerticalAngle = -45f, maxVerticalAngle = 45f;

        [SerializeField, Min(0f)]
        public float alignDelay = 5f;

        [SerializeField, Range(0f, 90f)]
        float alignSmoothRange = 45f;

        [SerializeField]
        LayerMask obstructionMask = -1;

        Camera regularCamera;

        Vector3 focusPoint, previousFocusPoint;

        Vector2 orbitAngles = new Vector2(45f, 0f);

        float lastManualRotationTime;

        [SerializeField]
        private float _distance = 5f;

        Vector3 CameraHalfExtends {
            get {
                Vector3 halfExtends;
                halfExtends.y =
                    regularCamera.nearClipPlane *
                    Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
                halfExtends.x = halfExtends.y * regularCamera.aspect;
                halfExtends.z = 0f;
                return halfExtends;
            }
        }

        void OnValidate () 
        {
            if (maxVerticalAngle < minVerticalAngle) 
                maxVerticalAngle = minVerticalAngle;
        }

        private void OnEnable() 
        {
            regularCamera = GetComponent<Camera>();
            focusPoint = player.transform.position + player.GetCameraOffset();
            transform.localRotation = Quaternion.Euler(orbitAngles);
            _distance = currentDistance;

            Cursor.lockState = CursorLockMode.Locked;
        }

        void LateUpdate () 
        {
            if(input["change_mouse"].boolDownValue)
                Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;

            currentDistance -= input["mouse_scroll"]/10f * mouseScrollSensitivity;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
            _distance = Mathf.Lerp(_distance, currentDistance, Time.deltaTime * 5f);

            UpdateFocusPoint();
            Quaternion lookRotation;
            if (ManualRotation() || AutomaticRotation()) 
            {
                ConstrainAngles();
                lookRotation = Quaternion.Euler(orbitAngles);
            }
            else
                lookRotation = transform.localRotation;
            

            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = focusPoint - lookDirection * _distance;

            Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
            Vector3 rectPosition = lookPosition + rectOffset;
            Vector3 castFrom = player.transform.position + player.GetCameraOffset();
            Vector3 castLine = rectPosition - castFrom;
            float castDistance = castLine.magnitude;
            Vector3 castDirection = castLine / castDistance;

            if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,lookRotation, castDistance, obstructionMask)) 
            {
                rectPosition = castFrom + castDirection * hit.distance;
                lookPosition = rectPosition - rectOffset;
            }
            
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        void UpdateFocusPoint () 
        {
            previousFocusPoint = focusPoint;
            Vector3 targetPoint = player.transform.position + player.GetCameraOffset();
            if (focusRadius > 0f) 
            {
                float distance = Vector3.Distance(targetPoint, focusPoint);
                float t = 1f;
                if (distance > 0.01f && focusCentering > 0f) {
                    t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
                }
                if (distance > focusRadius) {
                    t = Mathf.Min(t, focusRadius / distance);
                }
                focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
            }
            else {
                focusPoint = targetPoint;
            }
        }

        bool ManualRotation () 
        {
            if(Cursor.lockState != CursorLockMode.Locked)
                return false;

            Vector2 input = new Vector2(this.input["mouse_y"] * mouseSensitivityY,this.input["mouse_x"] * mouseSensitivityX);

            const float e = 0.001f;
            if (input.x < -e || input.x > e || input.y < -e || input.y > e) 
            {
                orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
                lastManualRotationTime = Time.unscaledTime;
                return true;
            }
            return false;
        }

        bool AutomaticRotation () 
        {
            if (Time.unscaledTime - lastManualRotationTime < alignDelay)
                return false;

            Vector2 movement = new Vector2(focusPoint.x - previousFocusPoint.x, focusPoint.z - previousFocusPoint.z);
            float movementDeltaSqr = movement.sqrMagnitude;
            if (movementDeltaSqr < 0.0001f) 
                return false;

            float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
            float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
            float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
            
            if (deltaAbs < alignSmoothRange)
                rotationChange *= deltaAbs / alignSmoothRange;
            else if (180f - deltaAbs < alignSmoothRange)
                rotationChange *= (180f - deltaAbs) / alignSmoothRange;

            orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
            return true;
        }

        void ConstrainAngles () 
        {
            orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

            if (orbitAngles.y < 0f) 
            {
                orbitAngles.y += 360f;
            }
            else if (orbitAngles.y >= 360f) 
            {
                orbitAngles.y -= 360f;
            }
        }

        static float GetAngle (Vector2 direction) 
        {
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0f ? 360f - angle : angle;
        }

        void Start(){
            Instance = this;
        }
    }
}