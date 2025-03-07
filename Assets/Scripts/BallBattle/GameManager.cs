﻿using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameParameters parameters;

    private int currentMatch = 1;
    private float timer;
    private bool isPlayerAttacking = true;

    private int playerWins = 0;
    private int enemyWins = 0;

    public TMP_Text timerText, matchText, playerGameStateText, enemyGameStateText;
    public GameObject playerField, enemyField;
    public GameObject ballPrefab;
    private GameObject ballInstance;

    void Awake() { Instance = this; }

    void Start()
    {
        StartMatch();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString();
        }
        else
        {
            EndMatch(false); // ❌ Time ran out → Match Draw
        }
    }

    void StartMatch()
    {
        if (currentMatch > parameters.matchesPerGame)
        {
            GameOver();
            return;
        }

        timer = parameters.matchTimeLimit;

        if (isPlayerAttacking)
        {
            playerGameStateText.text = "PLAYER - ATTACKING";
            enemyGameStateText.text = "ENEMY - DEFENDING";
            SpawnBall(enemyField); // ✅ Ball spawns in the Enemy Field when Player attacks
        }
        else
        {
            playerGameStateText.text = "PLAYER - DEFENDING";
            enemyGameStateText.text = "ENEMY - ATTACKING";
            SpawnBall(playerField); // ✅ Ball spawns in the Player Field when Enemy attacks
        }

        matchText.text = "Match " + currentMatch;
        isPlayerAttacking = !isPlayerAttacking;
        currentMatch++;
    }

    void SpawnBall(GameObject field)
    {
        if (ballInstance != null) Destroy(ballInstance); // Remove old ball

        Collider fieldCollider = field.GetComponent<Collider>();
        if (fieldCollider == null)
        {
            Debug.LogError("❌ Field collider is missing on " + field.name);
            return;
        }

        Vector3 fieldCenter = fieldCollider.bounds.center;
        Vector3 fieldSize = fieldCollider.bounds.size;

        float minX = fieldCenter.x - (fieldSize.x / 2) + 1f;
        float maxX = fieldCenter.x + (fieldSize.x / 2) - 1f;
        float minZ = fieldCenter.z - (fieldSize.z / 2) + 1f;
        float maxZ = fieldCenter.z + (fieldSize.z / 2) - 1f;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float ballHeight = fieldCollider.bounds.max.y + 0.08f;

        Vector3 spawnPosition = new Vector3(randomX, ballHeight, randomZ);

        ballInstance = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        ballInstance.tag = "Ball";

        Debug.Log("✅ Ball Spawned at: " + spawnPosition);
    }

    // ✅ Handles Match End: Timeout = Draw, Goal = Win
    public void EndMatch(bool attackerWon)
    {
        if (attackerWon)
        {
            if (isPlayerAttacking)
            {
                playerWins++;
                playerGameStateText.text = "PLAYER - WIN 🎉";
                enemyGameStateText.text = "ENEMY - LOSE ❌";
            }
            else
            {
                enemyWins++;
                playerGameStateText.text = "PLAYER - LOSE ❌";
                enemyGameStateText.text = "ENEMY - WIN 🎉";
            }
        }
        else
        {
            playerGameStateText.text = "MATCH DRAW!";
            enemyGameStateText.text = "MATCH DRAW!";
        }

        Debug.Log($"Match Result: Player Wins = {playerWins}, Enemy Wins = {enemyWins}");

        Invoke(nameof(StartMatch), 2f);
    }

    // ✅ Handles Game Over after 5 Matches
    void GameOver()
    {
        if (playerWins > enemyWins)
        {
            playerGameStateText.text = "🎉 PLAYER WINS THE GAME!";
            enemyGameStateText.text = "❌ ENEMY LOSES!";
        }
        else if (enemyWins > playerWins)
        {
            playerGameStateText.text = "❌ PLAYER LOSES!";
            enemyGameStateText.text = "🎉 ENEMY WINS THE GAME!";
        }
        else
        {
            playerGameStateText.text = "🏆 GAME TIED! EXTRA ROUND?";
            enemyGameStateText.text = "🏆 GAME TIED!";
        }

        Debug.Log("🏆 Game Over: Final Score -> Player: " + playerWins + " | Enemy: " + enemyWins);
    }

    public bool IsPlayerAttacking()
    {
        return isPlayerAttacking;
    }
}
