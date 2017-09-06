using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRPuzzler
{
    [CreateAssetMenu(menuName = "Sequences/Sequence")]
    public class SequenceTemplate : ScriptableObject
    {
        
        public GameObject[] SequenceReceivers;
        public int[] Sequence;
     
    }
}