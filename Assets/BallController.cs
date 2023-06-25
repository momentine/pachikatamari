/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rollSpeed;
    [SerializeField] private Rigidbody rb;
    private float size = 2;

    void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 movement = (input.z * cameraTransform.forward) + (input.x * cameraTransform.right);
        rb.AddForce(movement * rollSpeed * Time.fixedDeltaTime * size);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("prop") && collision.transform.localScale.magnitude <= size)
        {
            collision.transform.parent = transform;
            size += collision.transform.localScale.magnitude;
        }
    }
}*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rollSpeed;
    [SerializeField] private Rigidbody rb;
    private float size = 2;
    private float fallTimer;
    private bool isFalling;

    void Start()
    {
        fallTimer = 0f;
        isFalling = false;
    }

    void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 movement = (input.z * cameraTransform.forward) + (input.x * cameraTransform.right);
        rb.AddForce(movement * rollSpeed * Time.fixedDeltaTime * size);

        // Check if the player is falling off the platform
        if (transform.position.y < -10f)
        {
            // Restart the game or perform other game-ending actions
            EndGame();
        }

        // Check if the player is falling without touching any object
        if (rb.velocity.y < 0 && !isFalling && !IsTouchingObject())
        {
            fallTimer += Time.fixedDeltaTime;
            if (fallTimer >= 3f)
            {
                // Restart the game or perform other game-ending actions
                EndGame();
            }
        }
        else
        {
            fallTimer = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("prop") && collision.transform.localScale.magnitude <= size)
        {
            collision.transform.parent = transform;
            size += collision.transform.localScale.magnitude;
        }
    }

    bool IsTouchingObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Adjust the radius as needed

        foreach (Collider collider in colliders)
        {
            if (!collider.CompareTag("player"))
            {
                return true;
            }
        }

        return false;
    }

    void EndGame()
    {
        // Restart the scene or perform other game-ending actions
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
