using UnityEngine;

public class LavaDeath : MonoBehaviour
{
    public ResetManager resetManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER EN LAVA");
            resetManager.ResetNivel();
        }
    }
}
