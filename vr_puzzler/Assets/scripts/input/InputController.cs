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
        public SphereCollider[] Blobs;
        public BoxCollider TutorialBlob;
         

        public void DisableInput()
        {
            Debug.Log("input disabled!");
            Pointer.enabled = false;
       
        }
        public void EnableInput()
        {
            Debug.Log("input enabled!");
           Pointer.enabled = true;
        }

        public void BlobInput(bool _valid)
        {
            foreach(SphereCollider _col in Blobs)
            {
               // Debug.Log("Blob input: " + _valid);
                _col.enabled = _valid;               
            }
        }
        public void TutorialBlobInput(bool _valid)
        {
            Debug.Log("TutorialBlobInput input: " + _valid);
            TutorialBlob.enabled = _valid;
        }
    }
}