using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPuzzler.Templates;

namespace VRPuzzler
{


    public class InputController : Singleton<InputController>
    {

        public GameObject EventSystem;

        public void DisableInput()
        {
            Debug.Log("input disabled!");
            EventSystem.SetActive(false);
        }
        public void EnableInput()
        {
            Debug.Log("input enabled!");
            EventSystem.SetActive(true);
        }

    }
}