using UnityEngine;

public class BloqueMortal : MonoBehaviour
{
    [Header("Configuración de Teletransporte")]
    [Tooltip("Coordenadas X, Y, Z a las que quieres enviar al jugador")]
    public Vector3 coordenadasDestino = new Vector3(0f, 1f, 0f);

    [Header("Configuración de Detección")]
    public string tagJugador = "Player";

    private void OnCollisionEnter(Collision choque)
    {
        if (choque.gameObject.CompareTag(tagJugador))
        {
            Debug.Log("Teletransportando a: " + coordenadasDestino);
            Teletransportar(choque.gameObject);
        }
    }

    private void Teletransportar(GameObject jugador)
    {
        // 1. Intentamos obtener el CharacterController (si existe)
        CharacterController cc = jugador.GetComponent<CharacterController>();

        if (cc != null)
        {
            // Si el jugador tiene CharacterController, hay que apagarlo para moverlo
            cc.enabled = false;
            jugador.transform.position = coordenadasDestino;
            cc.enabled = true;
        }
        else
        {
            // Si no tiene CC (es un Rigidbody o un objeto simple)
            jugador.transform.position = coordenadasDestino;
        }
    }
}