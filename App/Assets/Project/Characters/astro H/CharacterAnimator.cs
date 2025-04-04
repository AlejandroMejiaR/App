using System.Collections;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(bool isSelected)
    {

        animator.SetBool("hola_t", isSelected);
    }
}