using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerController : MonoBehaviour
	{
		private Animator animator;
        private CharacterController controller;
        private Vector3 input = Vector3.zero;
        public Vector3 velocity = Vector3.zero;
        public float jumpForce = 20f;
        public float walkSpeed = 5f;
        public float walkAcceleration = 8f;
        public float rotationSpeed = 90f;
        public bool isGrounded = false;
        public Transform target;

        public void TeleportTo(Vector3 target ) => controller.Move(target - transform.position);
        public void TeleportCity() => TeleportTo(new Vector3(511.0f,51.169f,844.0f));
        public void TeleportCity2() => TeleportTo(new Vector3(692.5f,27.19f,482.9f));
        //public GameObject meshPlayer;

	    //Metodo Start e executado uma unica vez, quando o script e executado.
		void Start() 
		{
			animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            animator.SetFloat("Velocity",0.0f);
		}

        private void FixedUpdate() {

            this.isGrounded = controller.isGrounded;
            //Gravity
            if(!controller.isGrounded)
            {
                velocity.y -= 9.8f * Time.fixedDeltaTime;
            }
            else
            {
                velocity.y = 0;
            }
            controller.Move(velocity * Time.fixedDeltaTime);
            
            
            
        }

		void Update() 
		{	

            Move();
		}
        
		public void Move()
        {
            Vector3 newInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            
            float v = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);
            float h = Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0);

            Vector3 input = new Vector3(h,0,v).normalized;
            
            Vector3 movement = walkSpeed * (transform.forward * input.z + transform.right * input.x);

            controller.Move(movement * Time.deltaTime);

            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y + input.x * rotationSpeed * Time.deltaTime * Mathf.Abs(input.z),0);

            animator.SetFloat("Velocity", input.z);

            if(Input.GetKey(KeyCode.Space) && isGrounded){  
                Debug.Log("Pulo");
                isGrounded = false;
                velocity += new Vector3(0f,jumpForce,0f);
            }
            if(Input.GetKey(KeyCode.LeftShift)){
                animator.SetBool("Run", true);  
            }else{
                
                animator.SetBool("Run", false);
            }
		}

        


   

       

	}
}
 