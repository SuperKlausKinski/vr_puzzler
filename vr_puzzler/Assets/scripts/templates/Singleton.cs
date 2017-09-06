// generic singleton class
using UnityEngine;
using System.Collections.Generic;
using VRPuzzler.Utilities;
using UnityEngine.Assertions;

namespace VRPuzzler.Templates
{


    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T m_instance = null;
        //--------------------------------------------------------------------------------------------------------
        public static T Instance
        {
            get { return getInstance(); }
        }
        //--------------------------------------------------------------------------------------------------------
        public virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
            }
            else
            {
                Debug.LogWarning("More than one " + typeof(T).ToString() + " in scene, deleted one on"+ gameObject.name);
                Destroy(gameObject.GetComponent(typeof(T).ToString()));
            }

            
        }
        //--------------------------------------------------------------------------------------------------------
        private static T getInstance()
        {
            return m_instance;
        }
        //--------------------------------------------------------------------------------------------------------
    }

}