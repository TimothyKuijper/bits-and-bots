using UnityEngine;

public class RankSystemTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            RankSystem.Win();

        if (Input.GetKeyDown(KeyCode.DownArrow))
            RankSystem.Loss();

        if (Input.GetKeyDown(KeyCode.R))
            RankSystem.ResetRank();

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log($"Rank: {RankSystem.CurrentRank} | XP: {RankSystem.CurrentXp}");
    }
}