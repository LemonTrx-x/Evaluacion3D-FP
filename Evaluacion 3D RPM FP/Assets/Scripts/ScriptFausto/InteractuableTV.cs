using UnityEngine;

public class InteractuableTV : MonoBehaviour
{
    [Header("Referencia al GameManager")]
    [Tooltip("Arrastra aquí el objeto que tiene el script MinijuegoTV")]
    public MinijuegoTV scriptMinijuego;

    // Esta función la ejecutará el jugador al pulsar E
    public void Encender()
    {
        Debug.Log("¡Encendiendo la TV!");
        scriptMinijuego.IniciarMinijuego();
    }
}
