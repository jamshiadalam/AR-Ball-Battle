using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("🎯 Goal Scored at: " + gameObject.name);
            GameManager.Instance.EndMatch(true); // ✅ Fix: Pass `true` to indicate Attacker wins
            Destroy(other.gameObject); // Remove Ball
        }
    }
}