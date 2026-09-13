using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;
public class FingerStateScript : MonoBehaviour
{
    public bool isHaveLoadFile = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OpenStatePanel()
    {
       // if(isHaveLoadFile)
            gameObject.SetActive(true);
            
    }
    public void CloseStatePanel()
    {
        //if (isHaveLoadFile)
            gameObject.SetActive(false);
    }
    
}
