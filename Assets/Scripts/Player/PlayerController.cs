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
            
            input = Vector3.Lerp(input, newInput, Time.deltaTime * walkAcceleration);
            
            Vector3 movement = walkSpeed * (transform.forward * input.z + transform.right * input.x);

            controller.Move(movement * Time.deltaTime);

            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y + input.x * rotationSpeed * Time.deltaTime * Mathf.Abs(input.z),0);

            animator.SetFloat("Velocity", input.z);

            if(Input.GetKey(KeyCode.Space) && isGrounded)
            {  
                isGrounded = false;
                velocity += new Vector3(0f,jumpForce,0f);
                animator.SetBool("Jump", true);
            }else{
                animator.SetBool("Jump", false);
            }

            if(Input.GetKey(KeyCode.LeftShift)){
                animator.SetBool("Run", true);  
            }else{
                
                animator.SetBool("Run", false);
            }
		}


   

       

	}
}
 