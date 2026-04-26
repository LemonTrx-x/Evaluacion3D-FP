using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public Animator animator;
    public TriggersCollider[] triggers;
    public Transform respawnPoint;
    public GameObject player;

    public void ResetNivel()
    {
        // 1. Reset Animator completo
        animator.Rebind();
        animator.Update(0f);

        // 2. Reset lógica de triggers
        foreach (var t in triggers)
        {
            t.ResetTrigger();
        }

        player.transform.position = respawnPoint.position;
    }
}
