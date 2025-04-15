using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RunnerControl : MonoBehaviour
{
    public float baseSpeed = 3f;
    public float lateralSpeed = 3f;
    public float jumpForce = 7f;
    public float speedIncreaseRate = 0.1f;
    private float currentSpeed;
    private Rigidbody rb;
    private bool isGrounded;
    private int score = 0;
    public Text scoreText;
    public GameObject coinParticle;
 void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = baseSpeed;
        scoreText.text = "Score: 0";
    }
    void Update()
    {
        Vector3 forwardMovement = Vector3.forward * currentSpeed;
        rb.MovePosition(rb.position + forwardMovement * Time.deltaTime);
        float moveX = Input.GetAxis("Horizontal") * lateralSpeed;
        Vector3 lateralMovement = new Vector3(moveX, 0f, 0f);
        rb.MovePosition(rb.position + lateralMovement * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        currentSpeed += speedIncreaseRate * Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Oyun Bitti! Skor: " + score);
            Time.timeScale = 0;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            score += 10;
            scoreText.text = "Score: " + score;
            GameObject coinEffect = Instantiate(coinParticle, other.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = coinEffect.GetComponent<ParticleSystem>();
            particleSystem.Play();
            StartCoroutine(StopParticleEffect(particleSystem, .5f));
            Destroy(particleSystem,1.5f);
            Destroy(other.gameObject);
        }
    }

    IEnumerator StopParticleEffect(ParticleSystem particleSystem, float delay)
    {
        yield return new WaitForSeconds(delay);
        particleSystem.Stop();
    }
}

