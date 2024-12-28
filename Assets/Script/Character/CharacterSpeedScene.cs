using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpeedScene : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetMove(float value,float maxValue)
    {
        animator.SetFloat("Speed", value/maxValue);
    }
}
