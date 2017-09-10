using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPuzzler.Templates;
using UnityEngine.EventSystems;
namespace VRPuzzler
{


    public class InputController : Singleton<InputController>
    {

        
        public GvrReticlePointer Pointer;

        public void DisableInput()
        {
            Debug.Log("input disabled!");
            //Pointer.enabled = false;
       
        }
        public void EnableInput()
        {
            Debug.Log("input enabled!");
           // Pointer.enabled = true;
        }

    }
}