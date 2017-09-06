using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class VisualFeedbackController : MonoBehaviour {

    public Text Text;
    private UnityAction listenForRightAnswer;
    private UnityAction listenForWrongAnswer;

    void Awake()
    {
        listenForRightAnswer = new UnityAction(RightAnswer);
        listenForWrongAnswer = new UnityAction(WrongAnswer);
    }

    void Start()
    {
        Text.text = "Waiting";
        EventManager.Instance.StartListening("RIGHT", listenForRightAnswer);
        EventManager.Instance.StartListening("WRONG", listenForWrongAnswer);
    }

    private void RightAnswer()
    {
      Text.text = "RIGHT!";
      DOVirtual.DelayedCall(1f,() => Text.text = "Waiting");
    }
    private void WrongAnswer()
    {
      Text.text = "Wrong!";
      DOVirtual.DelayedCall(1f, () => Text.text = "Waiting");
    }
}
