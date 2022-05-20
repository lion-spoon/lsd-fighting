using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    private Animator animatorFight;
    // Start is called before the first frame update
    void Start()
    {
        animatorFight = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BoxeStylish();
    }

    public void BoxeStylish(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            animatorFight.SetBool("dodging", true);
        }else{
            animatorFight.SetBool("dodging", false);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)){
            animatorFight.SetBool("center_block", true);
        }else{
            animatorFight.SetBool("center_block", false);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){
            animatorFight.SetBool("punch", true);
        }else{
            animatorFight.SetBool("punch", false);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1)){
            animatorFight.SetBool("Hook", true);
        }else{
            animatorFight.SetBool("Hook", false);
        }
    }
}
