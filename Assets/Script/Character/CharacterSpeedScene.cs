using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpeedScene : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }
}
