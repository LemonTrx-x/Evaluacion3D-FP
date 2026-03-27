using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Destino del Teletransporte")]
    public Vector3 coordenadasDestino = new Vector3(10f, 1f, 10f);

    // Función pública que el jugador llamará cuando pulse la 'E'
    public void EjecutarAccion(Transform jugador)
    {
        jugador.position = coordenadasDestino;
    }
}