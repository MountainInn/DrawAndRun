using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Gem : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider _collider;

    public void Collect()
    {
        Debug.Log($"Collect");

        animator.SetTrigger("collectTrigger");

        _collider.enabled = false;
    }
}
