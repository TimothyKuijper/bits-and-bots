using UnityEngine;
using Yakapedia;

public class RankSystem
{
    private const string RankKey = "Rank";
    private const int BaseRank = 0;
    private const int BaseXp = 100;
    private const float XPModifier = 0.01f;

    public static int CurrentRank => PersistentData.Get(RankKey, BaseRank);

    public static void RankUp(int plusRank) => PersistentData.Set(RankKey, CurrentRank + plusRank);

    public static void RankDown(int minRank)
    {
        int newRank = Mathf.Clamp(CurrentRank - minRank, BaseRank, CurrentRank);
        PersistentData.Set(RankKey, newRank);
    }

    public static void ResetRank() => PersistentData.Set(RankKey, BaseRank);
}