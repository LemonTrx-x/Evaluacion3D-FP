using UnityEngine;

public class BotonLejos : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;

    [Header("Configuración")]
    [Tooltip("Distancia a la que el objeto reaccionará")]
    public float radioDeteccion = 3f;

    [Tooltip("Coordenadas a las que se moverá el objeto")]
    public Vector3 coordenadasDestino;

    void Update()
    {
        if (jugador == null) return;

        // Calculamos la distancia entre el objeto y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador entra en el radio...
        if (distancia < radioDeteccion)
        {
            TeletransportarObjeto();
        }
    }

    void TeletransportarObjeto()
    {
        Debug.Log("¡Jugador detectado! El objeto se mueve a: " + coordenadasDestino);
        transform.position = coordenadasDestino;

        // Opcional: Desactivar este script si solo quieres que se mueva una vez
        // this.enabled = false;
    }

    // Dibujamos el radio en el Editor para poder verlo visualmente
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}