using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

public class GameController : MonoBehaviour
{
    public GameObject[] playerSkin;
    public int currentSkin;
    public float teste = 2;
    // Start is called before the first frame update
    void Start()
    {
        playerSkin[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextSkin(){
        playerSkin[currentSkin].SetActive(false);
        currentSkin = (currentSkin + 1) %  playerSkin.Length;
        playerSkin[currentSkin].SetActive(true);
        OrbitCamera.Instance.player = playerSkin[currentSkin].GetComponent<PlayerController>();
    }

    public void PreviousSkin(){
        playerSkin[currentSkin].SetActive(false);
        currentSkin--;
        
        if(currentSkin < 0)
        {
            
            currentSkin += playerSkin.Length;
        }
        playerSkin[currentSkin].SetActive(true);
        OrbitCamera.Instance.player = playerSkin[currentSkin].GetComponent<PlayerController>();
    }

    

}
