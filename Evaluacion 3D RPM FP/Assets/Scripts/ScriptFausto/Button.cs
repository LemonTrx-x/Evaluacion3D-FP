using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public Vector3 coordenadasDestino = new Vector3(10f, 1f, 10f);
    public string textoMostrar = "Nivel 2: El cangrejo";

    [Header("Referencias (¡Arrastra el Canvas aquí!)")]
    public TransicionManager managerTransicion;

    // Función que llama el jugador al pulsar la 'E'
    public void EjecutarAccion(Transform jugador)
    {
        // Si conectaste el Canvas en el Inspector, iniciamos el efecto
        if (managerTransicion != null)
        {
            managerTransicion.IniciarTransicion(jugador, coordenadasDestino, textoMostrar);
        }
        else
        {
            Debug.LogError("¡Se te olvidó arrastrar el Canvas al script del Botón!");
        }
    }
}