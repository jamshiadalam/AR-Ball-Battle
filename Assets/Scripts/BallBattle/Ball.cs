using UnityEngine;

public class Ball : MonoBehaviour
{
    private Transform carrier;
    public float passSpeed = 5f;

    void Update()
    {
        if (carrier != null)
            transform.position = Vector3.Lerp(transform.position, carrier.position, Time.deltaTime * passSpeed);
    }

    public void SetCarrier(Transform newCarrier)
    {
        carrier = newCarrier;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.name); // Debugging

        if (other.CompareTag("Goal")) // ✅ Ensure "Goal" is a valid tag
        {
            Debug.Log("GOAL SCORED!");
            GameManager.Instance.EndMatch(true); // ✅ Fix: Pass `true` to indicate Attacker wins
            Destroy(gameObject); // Remove the Ball after goal
        }
    }
}
