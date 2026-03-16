using UnityEngine;

public class Test2 : MonoBehaviour
{
    public Test1 MyTest1;

    void OnEnable()
    {
        MyTest1.MyTest1Action += IncreaseAnotherFloat;
    }

    void OnDisable()
    {
        MyTest1.MyTest1Action -= IncreaseAnotherFloat;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //MyTest1.MyTestFloat = MyTest1.MyTestFloat + 0.1f;
        MyTest1.IncrementMyTestFloat();
    }

    private void IncreaseAnotherFloat()
    {
        
    }
}
