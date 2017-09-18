using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPuzzler.Templates;
using UnityEngine.Events;
using DG.Tweening;

namespace VRPuzzler
{
    public class GameController : Singleton<GameController>
    {

        private UnityAction listenForTutorialComplete;
        private UnityAction listenForExit;
        private UnityAction listenForChange;
        private UnityAction listenForSequenceCompleted;
        private UnityAction listenForAllSequencesCompleted;

        public int Score { get; private set; }

        new void Awake()
        {
            base.Awake();
            // listeners
            listenForExit = new UnityAction(OnGameStateExit);
            listenForChange = new UnityAction(OnGameStateChange);
            listenForSequenceCompleted = new UnityAction(IncreaseScore);
            listenForTutorialComplete = new UnityAction(TutorialComplete);
            listenForAllSequencesCompleted = new UnityAction(AllSequencesCompleted);
        }

        void Start()
        {
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", listenForChange);
            EventManager.Instance.StartListening("INPUTSEQUENCE_COMPLETED", listenForSequenceCompleted);
            EventManager.Instance.StartListening("TUTORIAL_COMPLETED", listenForTutorialComplete);
            EventManager.Instance.StartListening("INPUTSEQUENCES_FINISHED", listenForAllSequencesCompleted);
            InputController.Instance.TutorialBlobInput(false);
            InputController.Instance.BlobInput(false);
            DOVirtual.DelayedCall(1, () => StartIntro());
        }

        private void Init()
        {
            Score = 0;
        }

        public void StartIntro()
        {
            // wait for a second, then start the intro
            DOVirtual.DelayedCall(1, () => GameFSM.Instance.Gamestate = GameFSM.GAMESTATES.INTRO);                 
        }

        private void IncreaseScore()
        {
            Score++;
        }

        private void TutorialComplete()
        {
            GameFSM.Instance.Gamestate = GameFSM.GAMESTATES.GAME;
        }
        private void AllSequencesCompleted()
        {
            GameFSM.Instance.Gamestate = GameFSM.GAMESTATES.FINISH;
        }
        private void OnGameStateChange()
        {

            switch (GameFSM.Instance.Gamestate)
            {
                case(GameFSM.GAMESTATES.INIT):                                   
                break;
                case (GameFSM.GAMESTATES.INTRO):
                    InputController.Instance.TutorialBlobInput(true);
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
