using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración de Sensibilidad")]
    public float sensibilidadRaton = 100f;

    [Header("Referencias")]
    public Transform cuerpoJugador;

    private float rotacionVertical = 0f;

    void Start()
    {
        // Ocultar el cursor y bloquearlo en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Búsqueda automática del cuerpo si se nos olvida asignarlo en el Inspector
        if (cuerpoJugador == null && transform.parent != null)
        {
            cuerpoJugador = transform.parent;
        }
    }

    void Update()
    {
        // Si no hay cuerpo asignado ni padre, salimos para evitar errores
        if (cuerpoJugador == null) return;

        // Capturar el movimiento del ratón
        float ratonX = Input.GetAxis("Mouse X") * sensibilidadRaton * Time.deltaTime;
        float ratonY = Input.GetAxis("Mouse Y") * sensibilidadRaton * Time.deltaTime;

        // Calcular y limitar la rotación vertical (arriba/abajo)
        rotacionVertical -= ratonY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -90f, 90f);

        // Aplicar la rotación a la cámara (cabeza)
        transform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);

        // Girar el cuerpo entero del jugador (izquierda/derecha)
        cuerpoJugador.Rotate(Vector3.up * ratonX);
    }
}