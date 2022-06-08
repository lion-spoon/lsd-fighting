using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combos : MonoBehaviour
{
    private Animator animator;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.9f;
    public int noOfClicksLight = 0;
    float lastClickedTimeLight = 0;
    public float maxComboDelayLight = 0.9f;
    public RuntimeAnimatorController[] newController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimator(int i){
        animator.runtimeAnimatorController = newController[i];
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastClickedTime > maxComboDelay){
            noOfClicks = 0;
        }
        if(Input.GetMouseButtonDown(1)){
            lastClickedTime = Time.time;
            noOfClicks++;
            if(noOfClicks == 1){
                animator.SetBool("heavy1", true);
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);
        }
        if(Time.time - lastClickedTimeLight > maxComboDelayLight){
            noOfClicksLight = 0;
        }
        if(Input.GetMouseButtonDown(0)){
            lastClickedTimeLight = Time.time;
            noOfClicksLight++;
            if(noOfClicksLight == 1){
                animator.SetBool("light1", true);
            }
            noOfClicksLight = Mathf.Clamp(noOfClicksLight, 0, 4);
        }
        
    }

    public void returnLight1(){
        if(noOfClicksLight >=2){
            animator.SetBool("light2", true);
        }else{
            animator.SetBool("light1", false);
            noOfClicksLight = 0;
        }
    }

    public void returnLight2(){
        if(noOfClicksLight >= 3){
            animator.SetBool("light3", true);
        }else{
            animator.SetBool("light1", false);
            animator.SetBool("light2", false);
            noOfClicksLight = 0;
        }	
    }

    public void returnLight3(){
        if(noOfClicksLight >=4){
            animator.SetBool("light4", true);
        }else{
            animator.SetBool("light1", false);
            animator.SetBool("light2", false);
            animator.SetBool("light3", false);
            noOfClicksLight = 0;
        }	
    }

    public void returnLight4(){
        animator.SetBool("light1", false);
        animator.SetBool("light2", false);
        animator.SetBool("light3", false);
        animator.SetBool("light4", false);
        noOfClicksLight = 0;
    }

    public void return1(){
        if(noOfClicks >= 2){
            animator.SetBool("heavy2", true);
        }else{
            animator.SetBool("heavy1", false);
            noOfClicks = 0;
        }
    }

    public void return2(){
        if(noOfClicks >= 3){
            animator.SetBool("heavy3", true);
        }else{
            animator.SetBool("heavy1", false);
            animator.SetBool("heavy2", false);
            noOfClicks = 0;
        }
    }

    public void return3(){
        if(noOfClicks >= 4){
            animator.SetBool("heavy4", true);
        }else{
            animator.SetBool("heavy1", false);
            animator.SetBool("heavy2", false);
            animator.SetBool("heavy3", false);
            noOfClicks = 0;
        }
    }

    public void return4(){
        animator.SetBool("heavy1", false);
        animator.SetBool("heavy2", false);
        animator.SetBool("heavy3", false);
        animator.SetBool("heavy4", false);
        noOfClicks = 0;
    }
}
