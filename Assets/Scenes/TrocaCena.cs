using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaCena : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CenaDiurna(){
        SceneManager.LoadScene("CenaDiurna");
    }
    public void CenaNoturna(){
        SceneManager.LoadScene("CenaNoturna");
    }
}
