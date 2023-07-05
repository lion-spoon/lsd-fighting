using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

public class ChangePlayer : MonoBehaviour
{
    public GameObject[] player;
    public GameObject[] ui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Woman(){
        OrbitCamera.Instance.player = player[0].GetComponent<PlayerController>();
        player[0].SetActive(true);
        ui[0].SetActive(true);
        player[1].SetActive(false);
        ui[1].SetActive(false);
    }

    public void Man(){
        OrbitCamera.Instance.player = player[1].GetComponent<PlayerController>();
        player[1].SetActive(true);
        ui[1].SetActive(true);
        player[0].SetActive(false);
        ui[0].SetActive(false);
    }
}
