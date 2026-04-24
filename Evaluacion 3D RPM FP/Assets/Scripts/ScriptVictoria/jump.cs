using UnityEngine;

public class a : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocidadCaida;
    private bool isGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobar si está en el suelo
        isGrounded = controller.isGrounded;

        if (isGrounded && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f; // Mantener pegado al suelo
        }

        // Movimiento horizontal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocidadCaida.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad
        velocidadCaida.y += gravity * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }
}
