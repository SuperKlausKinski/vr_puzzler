using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAnimations : MonoBehaviour
{

    public enum BlobStates { IDLE, JUMP, MISS, WIN, HOVER, SING }


    [Header("Blob")]
    [SerializeField]
    private Animator m_Animator;

    private Dictionary<BlobStates, string[]> m_blobAnimationTrigger = new Dictionary<BlobStates, string[]>();

    void Awake()
    {
        m_blobAnimationTrigger.Add(BlobStates.HOVER, new string[] { "FOCUS" });
        m_blobAnimationTrigger.Add(BlobStates.IDLE, new string[] { "IDLE_00", "IDLE_01" });
        m_blobAnimationTrigger.Add(BlobStates.JUMP, new string[] { "JUMP" });
        m_blobAnimationTrigger.Add(BlobStates.MISS, new string[] { "MISS_00", "MISS_01" });
        m_blobAnimationTrigger.Add(BlobStates.WIN, new string[] { "RAISE_HANDS", "JUMP" });
        m_blobAnimationTrigger.Add(BlobStates.SING, new string[] { "SING_00", "SING_01", "SING_02" });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
