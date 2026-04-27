using UnityEngine;

public class TriggersCollider : MonoBehaviour
{
    public Animator animator;
    public string ActivarCollider;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.CompareTag("Player"))
        {
            Debug.Log("Entró en trigger: " + ActivarCollider);
            animator.SetTrigger(ActivarCollider);
            activado = true;
        }
    }
}
