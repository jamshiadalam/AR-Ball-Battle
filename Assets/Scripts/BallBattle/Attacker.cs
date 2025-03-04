using UnityEngine;

public class Attacker : MonoBehaviour
{
    private Transform ball;
    private Transform targetGate;
    public float speed = 3f;
    private bool hasBall = false;
    private bool isActive = false;
    private Renderer attackerRenderer;
    private Collider attackerCollider;

    void Start()
    {
        // 🔹 Find the Ball in the scene if not already assigned
        if (ball == null)
        {
            GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
            if (ballObj != null) ball = ballObj.transform;
        }

        // 🔹 Find the Target Gate (EnemyGate or PlayerGate)
        if (GameManager.Instance.IsPlayerAttacking())
        {
            GameObject enemyGateObj = GameObject.FindGameObjectWithTag("EnemyGate");
            if (enemyGateObj != null) targetGate = enemyGateObj.transform;
        }
        else
        {
            GameObject playerGateObj = GameObject.FindGameObjectWithTag("PlayerGate");
            if (playerGateObj != null) targetGate = playerGateObj.transform;
        }

        attackerRenderer = GetComponent<Renderer>();
        attackerCollider = GetComponent<Collider>();

        Invoke(nameof(Activate), 0.5f);
    }

    void Activate()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive || ball == null || targetGate == null) return;

        Transform target = hasBall ? targetGate : ball;
        float moveSpeed = hasBall ? 0.75f * Time.deltaTime : 1.5f * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
    }

    public void PickUpBall()
    {
        hasBall = true;
    }

    public void Deactivate(float reactivateTime)
    {
        isActive = false;
        attackerRenderer.material.color = Color.gray; // Greyscale effect
        attackerCollider.enabled = false; // Disable collisions temporarily

        Invoke(nameof(Reactivate), reactivateTime);
    }

    void Reactivate()
    {
        isActive = true;
        attackerRenderer.material.color = Color.white; // Restore color
        attackerCollider.enabled = true; // Re-enable collisions
    }
}