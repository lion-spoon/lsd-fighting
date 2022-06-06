using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeScenery();
    }

    public void ChangeScenery(){
        if(Input.GetKeyDown(KeyCode.T)){
            Instantiate(player[0], new Vector3(511.0f,51.169f,844.0f), Quaternion.identity);
            Destroy(player[1]);
        }
        if(Input.GetKeyDown(KeyCode.M)){
            Destroy(player[0]);
            Instantiate(player[1], new Vector3(692.5f,27.19f,482.9f), Quaternion.identity);
        }
    
    }
}
