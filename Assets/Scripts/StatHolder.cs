public static class StatHolder
{
    private static int howManyPlayers, roundNumber, witchSet, player1Color, player2Color, player3Color, player4Color, teamRedWins, teamBlueWins, player1SkinColor, player2SkinColor, player3SkinColor, player4SkinColor;
    private static float player1Wins, player2Wins, player3Wins, player4Wins, winsNeeded;
    public enum Modes { DM, TDM};
    public static Modes CurrentMode = Modes.DM;



    public static float Player1Wins
    {
        get
        {
            return player1Wins;
        }
        set
        {
            player1Wins = value;
        }
    }
    public static float Player2Wins
    {
        get
        {
            return player2Wins;
        }
        set
        {
            player2Wins = value;
        }
    }
    public static float Player3Wins
    {
        get
        {
            return player3Wins;
        }
        set
        {
            player3Wins = value;
        }
    }
    public static float Player4Wins
    {
        get
        {
            return player4Wins;
        }
        set
        {
            player4Wins = value;
        }
    }
    public static int TeamRedWins
    {
        get
        {
            return teamRedWins;
        }
        set
        {
            teamRedWins = value;
        }
    }
    public static int TeamBlueWins
    {
        get
        {
            return teamBlueWins;
        }
        set
        {
            teamBlueWins = value;
        }
    }
    public static int HowManyPlayers
    {
        get
        {
            return howManyPlayers;
        }
        set
        {
            howManyPlayers = value;
        }
    }

    public static float WinsNeeded
    {
        get
        {
            return winsNeeded;
        }
        set
        {
            winsNeeded = value;
        }
    }
    public static int RoundNumber
    {
        get
        {
            return roundNumber;
        }
        set
        {
            roundNumber = value;
        }
    }
    public static int WitchSet
    {
        get
        {
            return witchSet;
        }
        set
        {
            witchSet = value;
        }
    }

    public static int Player1Color
    {
        get
        {
            return player1Color;
        }
        set
        {
            player1Color = value;
        }
    }
    public static int Player2Color
    {
        get
        {
            return player2Color;
        }
        set
        {
            player2Color = value;
        }
    }
    public static int Player3Color
    {
        get
        {
            return player3Color;
        }
        set
        {
            player3Color = value;
        }
    }
    public static int Player4Color
    {
        get
        {
            return player4Color;
        }
        set
        {
            player4Color = value;
        }
    }
    public static int Player1SkinColor
    {
        get
        {
            return player1SkinColor;
        }
        set
        {
            player1SkinColor = value;
        }
    }
    public static int Player2SkinColor
    {
        get
        {
            return player2SkinColor;
        }
        set
        {
            player2SkinColor = value;
        }
    }
    public static int Player3SkinColor
    {
        get
        {
            return player3SkinColor;
        }
        set
        {
            player3SkinColor = value;
        }
    }
    public static int Player4SkinColor
    {
        get
        {
            return player4SkinColor;
        }
        set
        {
            player4SkinColor = value;
        }
    }
}