using UnityEngine;
using Yakapedia;

public class RankSystem
{
    private const string RankKey = "Rank";
    private const string XpKey = "XP";
    public const int BaseRank = 0;
    public const int XpThreshold = 100;
    public const int BaseXpGain = 25;
    public const int XpLoss = 15;
    public const float XpDecayPerRank = 0.025f;

    public static int CurrentRank => PersistentData.Get(RankKey, BaseRank);
    public static int CurrentXp => PersistentData.Get(XpKey, 0);
    private static int XpGain => Mathf.Max(1, Mathf.RoundToInt(BaseXpGain * (1f - CurrentRank * XpDecayPerRank)));

    public static void RankUp(int plusRank = 1) => PersistentData.Set(RankKey, CurrentRank + plusRank);

    public static void RankDown(int minRank = 1)
    {
        int newRank = Mathf.Clamp(CurrentRank - minRank, BaseRank, CurrentRank);
        PersistentData.Set(RankKey, newRank);
    }

    public static void Win()
    {
        int newXp = CurrentXp + XpGain;
        if (newXp >= XpThreshold)
        {
            RankUp();
            PersistentData.Set(XpKey, 0);
            return;
        }
        PersistentData.Set(XpKey, newXp);
    }

    public static void Loss()
    {
        int newXp = CurrentXp - XpLoss;
        if (newXp <= 0)
        {
            RankDown();
            PersistentData.Set(XpKey, 0);
            return;
        } 
        PersistentData.Set(XpKey, newXp);
    }

    public static void ResetRank()
    {
        PersistentData.Set(RankKey, BaseRank);
        PersistentData.Set(XpKey, 0);
    }
}