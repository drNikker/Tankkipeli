public static class StatHolder
{
    private static int player1Wins, player2Wins, howManyPlayers;

    public static int Player1Wins 
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
    public static int Player2Wins
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

}