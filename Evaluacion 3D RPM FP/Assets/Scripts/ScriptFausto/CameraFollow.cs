using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración de Sensibilidad")]
    public float sensibilidadRaton = 100f;

    private float rotacionVertical = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate() 
    {
        // Solo capturamos el eje Y del ratón (arriba/abajo)
        float ratonY = Input.GetAxis("Mouse Y") * sensibilidadRaton * Time.deltaTime;

        rotacionVertical -= ratonY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -90f, 90f);

        // Solo rotamos la cámara localmente
        transform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
    }
}