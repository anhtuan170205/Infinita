using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform gameModel;

    // Max values
    private float maxSteerVelocity = 2f;
    private float maxFowardVelocity = 30f;

    // Multipliers
    private float accelerationMultiplier = 3f;
    private float breakMultiplier = 15f;
    private float steerMultiplier = 5f;
    private Vector2 input = Vector2.zero;

    private void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);
    }

    private void FixedUpdate()
    {
        if (input.y > 0)
        {
            Accelerate();
        }
        else
        {
            rb.linearDamping = 0.2f;
        }
        if (input.y < 0)
        {
            Break();
        }
        Steer();

        if (rb.linearVelocity.z < 0)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void Accelerate()
    {
        rb.linearDamping = 0;
        if (rb.linearVelocity.z > maxFowardVelocity)
        {
            return;
        }
        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);
    }

    private void Break()
    {
        if (rb.linearVelocity.z < 0)
        {
            return;
        }
        rb.AddForce(rb.transform.forward * breakMultiplier * input.y);
    }

    private void Steer()
    {
        if (Mathf.Abs(input.x) > 0f)
        {
            float speedBaseSteerLimit = rb.linearVelocity.z / 5.0f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            rb.AddForce(rb.transform.right * steerMultiplier * input.x * speedBaseSteerLimit);

            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);

            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.linearVelocity.z);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 newInput)
    {
        newInput.Normalize();
        input = newInput;
    }
}
