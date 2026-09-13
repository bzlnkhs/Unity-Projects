using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HI5_DontDestroy : MonoBehaviour
{
    public bool isDontDestroy = true;
    // Start is called before the first frame update
    void Start()
    {
        if (isDontDestroy)
            DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
