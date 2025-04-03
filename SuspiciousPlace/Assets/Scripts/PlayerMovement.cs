using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variáveis:
    public float speed;
    public float horizontalInput;
    public float forwardInput;
    public float jumpForce = 5.0f;
    private Rigidbody playerRb;
    public bool isOnGround = true;
    public float runSpeed = 10.0f;
    public float walkSpeed = 6.0f;

    //movimentação da câmera:
    public float mouseSensitivity = 2.0f;  // Sensibilidade do mouse
    private float xRotation = 0f;  // Armazena a rotação no eixo X da câmera
    public Transform playerCamera;  // Referência para a câmera do jogador

    void Start()
    {
        //garante acesso ao rigidbody do player:
        playerRb = GetComponent<Rigidbody>();

        //desativa o cursor do mouse para que ele não saia da tela:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //movimentação do jogador:
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        //move o jogador:
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        //lógica de pulo:
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        //movimentação da câmera:
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;  // Rotação horizontal (eixo Y)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;  // Rotação vertical (eixo X)

        //rotação horizontal do jogador:
        transform.Rotate(Vector3.up * mouseX);

        //rotação vertical da câmera:
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Impede que a câmera gire demais para cima ou para baixo
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //lógica de correr:
        if (isOnGround) {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
