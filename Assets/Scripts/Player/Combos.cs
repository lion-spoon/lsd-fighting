using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combos : MonoBehaviour
{
    private Animator animator;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastClickedTime > maxComboDelay){
            noOfClicks = 0;
        }
        if(Input.GetMouseButtonDown(0)){
            lastClickedTime = Time.time;
            noOfClicks++;
            if(noOfClicks == 1){
                animator.SetBool("WeakPunch", true);
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }
    }

    public void return1(){
        if(noOfClicks >= 2){
            animator.SetBool("HeavyPunch", true);
        }else{
            animator.SetBool("WeakPunch", false);
            noOfClicks = 0;
        }
    }

    public void return2(){
        if(noOfClicks >= 3){
            animator.SetBool("Trip", true);
        }else{
            animator.SetBool("WeakPunch", false);
            animator.SetBool("HeavyPunch", false);
            noOfClicks = 0;
        }
    }

    public void return3(){

        animator.SetBool("WeakPunch", false);
        animator.SetBool("HeavyPunch", false);
        animator.SetBool("Trip", false);
        noOfClicks = 0;

    }
}
