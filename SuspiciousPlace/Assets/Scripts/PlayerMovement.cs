using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Vari�veis:
    public float speed = 7.0f;
    public float horizontalInput;
    public float forwardInput;
    public float jumpForce = 5.0f;
    private Rigidbody playerRb;
    public bool isOnGround = true;

    //movimenta��o da c�mera:
    public float mouseSensitivity = 2.0f;  // Sensibilidade do mouse
    private float xRotation = 0f;  // Armazena a rota��o no eixo X da c�mera
    public Transform playerCamera;  // Refer�ncia para a c�mera do jogador

    //audio do movimento de andar do jogador:
    public AudioSource footstepSound;

    void Start()
    {
        //garante acesso ao rigidbody do player:
        playerRb = GetComponent<Rigidbody>();

        //desativa o cursor do mouse para que ele n�o saia da tela:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //movimenta��o do jogador:
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        //tentativa de fazer audio ao andar do player:
        if (horizontalInput > 0 || forwardInput > 0) {
            footstepSound.enabled = true;
        }
        else
        {
            footstepSound.enabled = false;
        }

        //move o jogador:
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        //l�gica de pulo:
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

            //ao pular, o player nao fara barulho de andar:
            footstepSound.enabled = false;
        }

        //movimenta��o da c�mera:
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;  // Rota��o horizontal (eixo Y)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;  // Rota��o vertical (eixo X)

        //rota��o horizontal do jogador:
        transform.Rotate(Vector3.up * mouseX);

        //rota��o vertical da c�mera:
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Impede que a c�mera gire demais para cima ou para baixo
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
