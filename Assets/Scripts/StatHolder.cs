﻿public static class StatHolder
{
    private static int player1Wins, player2Wins, howManyPlayers, roundNumber = 1, witchSet, winsNeeded = 3;

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

    public static int WinsNeeded
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
}