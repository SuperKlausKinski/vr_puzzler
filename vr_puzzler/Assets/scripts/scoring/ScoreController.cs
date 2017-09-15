using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace VRPuzzler
{
    public class ScoreController : MonoBehaviour
    {
        public TextMeshProUGUI Text;
       
        private UnityAction listenForScoreIncrease;

        void Awake()
        {
            listenForScoreIncrease = new UnityAction(IncreaseScore);
        }

        void Start()
        {
            EventManager.Instance.StartListening("INPUTSEQUENCE_COMPLETED", listenForScoreIncrease);
        }

        public void IncreaseScore()
        {
            Debug.Log("SCORE INCREASED!!!");
            Text.text = (GameController.Instance.Score + 1).ToString();
          
        }


    }
}