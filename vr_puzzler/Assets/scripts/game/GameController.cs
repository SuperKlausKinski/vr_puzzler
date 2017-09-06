using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPuzzler.Templates;
using UnityEngine.Events;

namespace VRPuzzler
{
    public class GameController : Singleton<GameController>
    {


        private UnityAction listenForExit;
        private UnityAction listenForChange;
        private UnityAction listenForSequenceCompleted;
 


        public int Score { get; private set; }

        new void Awake()
        {
            base.Awake();
            listenForExit = new UnityAction(OnGameStateExit);
            listenForChange = new UnityAction(OnGamstateChange);
            listenForSequenceCompleted = new UnityAction(IncreaseScore);
        }

        void Start()
        {
            Debug.Log("start!");
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", listenForChange);
            EventManager.Instance.StartListening("INPUTSEQUENCE_COMPLETED", listenForSequenceCompleted);

        }

        private void Init()
        {
            Score = 0;
        }

        public void StartGame()
        {
            GameFSM.Instance.Gamestate = GameFSM.GAMESTATES.GAME;
        }

        private void IncreaseScore()
        {
            Score++;
        }

        void OnGamstateChange()
        {

            switch (GameFSM.Instance.Gamestate)
            {
                case(GameFSM.GAMESTATES.INIT):                                   
                break;
                case (GameFSM.GAMESTATES.INTRO):
                    break;
                case (GameFSM.GAMESTATES.LOADING):
                    break;
                case (GameFSM.GAMESTATES.START):
                    break;
                case (GameFSM.GAMESTATES.GAME):                   
                    break;
                case (GameFSM.GAMESTATES.FINISH):
                    break;
            }
        }
        void OnGameStateExit()
        {

            if (GameFSM.Instance.PrevState == GameFSM.GAMESTATES.LOADING)
            {
                Debug.Log("Exit Loading!");
            }
            if (GameFSM.Instance.PrevState == GameFSM.GAMESTATES.START)
            {
                Debug.Log("Exit Start!");
            }
        }
    }
}
