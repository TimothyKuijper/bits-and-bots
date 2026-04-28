using UnityEngine;

public class RankSystemTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            RankSystem.RankUp();

        if (Input.GetKeyDown(KeyCode.DownArrow))
            RankDown();

        if (Input.GetKeyDown(KeyCode.R))
            RankSystem.ResetRank();
    }

    private void RankDown()
    {
        int before = RankSystem.CurrentRank;
        RankSystem.RankDown();
        if (before == 0)
            Debug.Log("Floor held — rank stayed at 0");
    }
}