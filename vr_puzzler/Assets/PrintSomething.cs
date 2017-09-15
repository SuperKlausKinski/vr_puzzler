using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrintSomething : MonoBehaviour
{

	public void PrintSomethingInConsole()
    {
        Debug.Log("Something");
    }
   
    void Update()
    {
        if (GvrControllerInput.ClickButton)
        {
            Debug.Log("Click!");
        }
    }


}
