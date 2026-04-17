using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;

    private Rigidbody rb;

    [Header("Animación")]
    private Animator animador;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animador = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Lectura de las teclas (WASD o Flechas)
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoZ = Input.GetAxis("Vertical");

        // 2. Movimiento físico
        Vector3 direccion = transform.right * movimientoX + transform.forward * movimientoZ;
        transform.position += direccion * velocidad * Time.deltaTime;

        // 3. Control de Animación
        if (direccion.magnitude > 0.1f)
        {
            // Pasa a la animación "Run"
            if (animador != null) animador.SetBool("seMueve", true);
        }
        else
        {
            // Vuelve a "Happy Idle"
            if (animador != null) animador.SetBool("seMueve", false);
        }
    }
}