using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody carRb;
    private float accelerationMultiplier = 3f;
    private float breakMultiplier = 15f;
    private float steerMultiplier = 5f;
    private Vector2 input = Vector2.zero;

    private void FixedUpdate()
    {
        if (input.y > 0)
        {
            Accelerate();
        }
        else
        {
            carRb.linearDamping = 0.2f;
        }
        if (input.y < 0)
        {
            Break();
        }
        Steer();
    }

    private void Accelerate()
    {
        carRb.linearDamping = 0;
        carRb.AddForce(carRb.transform.forward * accelerationMultiplier * input.y);
    }

    private void Break()
    {
        if (carRb.linearVelocity.z < 0)
        {
            return;
        }
        carRb.AddForce(carRb.transform.forward * breakMultiplier * input.y);
    }

    private void Steer()
    {
        if (Mathf.Abs(input.x) > 0f)
        {
            carRb.AddForce(carRb.transform.right * steerMultiplier * input.x);
        }
    }

    public void SetInput(Vector2 newInput)
    {
        newInput.Normalize();
        input = newInput;
    }
}
