using System;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public float myTestFloat;
    
    public event Action MyTest1Action;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IncrementMyTestFloat();
    }

    public void IncrementMyTestFloat()
    {
        myTestFloat++;
        MyTest1Action?.Invoke();
    }
}
