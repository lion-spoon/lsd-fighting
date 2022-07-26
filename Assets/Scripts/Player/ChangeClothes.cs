using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothes : MonoBehaviour
{
    public GameObject[] hair;
    public GameObject[] pants;
    public GameObject[] accessories;
    public GameObject[] tShirt;
    public GameObject[] body;
    public GameObject[] eyeBrow;
    public GameObject[] boots;
    public int i_hair=0;
    public int i_pants=0;
    public int i_acc=0;
    public int i_shirt;
    public int i_body;
    public int i_eyeBrow;
    public int i_boots;

    // Start is called before the first frame update
    void Start()
    {
        hair[i_hair].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(i_hair>(hair.Length-1)){
            hair[0].SetActive(true);
            i_hair=0;
        }
        if(i_hair<0){
            hair[hair.Length-1].SetActive(true);
            i_hair = hair.Length-1;
        }
        if(i_pants>pants.Length-1){
            pants[0].SetActive(true);
            i_pants=0;
        }
        if(i_pants<0){
            pants[pants.Length-1].SetActive(true);
            i_pants = pants.Length-1;
        }
        if(i_acc>accessories.Length-1){
            accessories[0].SetActive(true);
            i_acc=0;
        }
        if(i_acc<0){
            accessories[accessories.Length-1].SetActive(true);
            i_acc = accessories.Length-1;
        }
        if(i_shirt>tShirt.Length-1){
            tShirt[0].SetActive(true);
            i_shirt=0;
        }
        if(i_shirt<0){
            tShirt[tShirt.Length-1].SetActive(true);
            i_shirt = tShirt.Length-1;
        }
        if(i_body>body.Length-1){
            body[0].SetActive(true);
            i_body=0;
        }
        if(i_body<0){
            body[body.Length-1].SetActive(true);
            i_body = body.Length-1;
        }
        if(i_eyeBrow>eyeBrow.Length-1){
            eyeBrow[0].SetActive(true);
            i_eyeBrow=0;
        }
        if(i_eyeBrow<0){
            eyeBrow[eyeBrow.Length-1].SetActive(true);
            i_eyeBrow = eyeBrow.Length-1;
        }
        if(i_boots>boots.Length-1){
            boots[0].SetActive(true);
            i_boots=0;
        }
        if(i_boots<0){
            boots[boots.Length-1].SetActive(true);
            i_boots = boots.Length-1;
        }
    }

    public void nextHair(){
        hair[i_hair].SetActive(false);
        i_hair++;
        hair[i_hair].SetActive(true);
        
    }
    public void previousHair(){
        hair[i_hair].SetActive(false);
        i_hair--;
        hair[i_hair].SetActive(true);
    }

    public void nextPants(){
        pants[i_pants].SetActive(false);
        i_pants++;
        pants[i_pants].SetActive(true);
    }
    public void previousPants(){
        pants[i_pants].SetActive(false);
        i_pants--;
        pants[i_pants].SetActive(true);
    }

    public void nextAcc(){
        accessories[i_acc].SetActive(false);
        i_acc++;
        accessories[i_acc].SetActive(true);
    }
    public void previousAcc(){
        accessories[i_acc].SetActive(false);
        i_acc--;
        accessories[i_acc].SetActive(true);
    }
    public void nextShirt(){
        tShirt[i_shirt].SetActive(false);
        i_shirt++;
        tShirt[i_shirt].SetActive(true);
    }
    public void previousShirt(){
        tShirt[i_shirt].SetActive(false);
        i_shirt--;
        tShirt[i_shirt].SetActive(true);
    }

    public void nextbody(){
        tShirt[i_body].SetActive(false);
        i_body++;
        tShirt[i_body].SetActive(true);
    }
    public void previousbody(){
        body[i_body].SetActive(false);
        i_body--;
        body[i_body].SetActive(true);
    }
    public void nextEyeBrow(){
        eyeBrow[i_eyeBrow].SetActive(false);
        i_eyeBrow++;
        eyeBrow[i_eyeBrow].SetActive(true);
    }
    public void previousEyeBrow(){
        eyeBrow[i_eyeBrow].SetActive(false);
        i_eyeBrow--;
        eyeBrow[i_eyeBrow].SetActive(true);
    }
    public void nextBoots(){
        boots[i_boots].SetActive(false);
        i_boots++;
        boots[i_boots].SetActive(true);
    }
    public void previousBoots(){
        boots[i_boots].SetActive(false);
        i_boots--;
        boots[i_boots].SetActive(true);
    }
}
