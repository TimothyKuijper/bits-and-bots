using UnityEngine;
using Yakapedia;

public class RankSystem : MonoBehaviour
{
    private const string RankKey = "Rank";
    private const int BaseRank = 0;

    public static int CurrentRank => PersistentData.Get(RankKey, BaseRank);

    public static void RankUp()
    {
        PersistentData.Set(RankKey, CurrentRank + 1);
        Debug.Log($"Ranked up! Current rank: {CurrentRank}");
    }

    public static void RankDown()
    {
        int newRank = Mathf.Max(0, CurrentRank - 1);
        PersistentData.Set(RankKey, newRank);
        Debug.Log($"Ranked down. Current rank: {CurrentRank}");
    }

    public static void ResetRank()
    {
        PersistentData.Set(RankKey, BaseRank);
        Debug.Log("Rank reset.");
    }
}