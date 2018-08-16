using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class RoundManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    AudioScript audioScript;

    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    public GameObject startMenu;

    //[HideInInspector]
    public List<Image> ScoreAmount1;
    [HideInInspector]
    public List<Image> ScoreAmount2;
    [HideInInspector]
    public List<Image> ScoreAmount3;
    [HideInInspector]
    public List<Image> ScoreAmount4;

    public List<Sprite> PlayerEmotions;
    public List<Sprite> PlayerTorsos;

    int playersAlive;
    [HideInInspector]
    public bool teamWon = false;


    public Text whoWonText;

    public GameObject roundWon;

    public int weaponSpawnTimer;
    [HideInInspector]
    public List<GameObject> alivePlayers;
    [HideInInspector]
    public List<GameObject> spawnPlayers;
    public List<GameObject> weaponSpawns;
    [HideInInspector]
    public List<GameObject> playerSpawns;
    [HideInInspector]
    public List<GameObject> redPlayers;
    [HideInInspector]
    public List<GameObject> bluePlayers;
    static Color Red = new Color(0.8113208f, 0.1339445f, 0.1519923f, 1);
    static Color Blue = new Color(0.245283f, 0.4317109f, 0.9811321f, 1);
    static Color Cyan = new Color(0.3163937f, 0.8490566f, 0.7672837f, 1);
    static Color Yellow = new Color(0.8509804f, 0.8040705f, 0.3176471f, 1);

    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
        sceneLoader = gameObject.GetComponent<SceneLoader>();
        //startMenu = GameObject.Find("Menu2");
        startMenu.SetActive(true);

        if (SceneManager.GetActiveScene().name != "JoiningScene")
        {
            if (StatHolder.HowManyPlayers == 0)
            {
                //This if statement exists for developing purposes. It ensures that the player spawns work even if you dont start at menu
                StatHolder.HowManyPlayers = 2;
            }
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                StatHolder.HowManyPlayers = 0;
            }
            playersForRound();
        }
        if (weaponSpawns.Count > 0)
        {
            StartCoroutine(SpawnWeapon());
        }
    }

    void playersForRound()
    {
        if (StatHolder.HowManyPlayers > 0)
        {
            if (playerSpawns.Count > 0)
            {
                switch (StatHolder.HowManyPlayers)
                {
                    case 2:
                        spawnPlayers.Add(playerPrefab1);
                        spawnPlayers.Add(playerPrefab2);
                        break;
                    case 3:
                        spawnPlayers.Add(playerPrefab1);
                        spawnPlayers.Add(playerPrefab2);
                        spawnPlayers.Add(playerPrefab3);
                        break;
                    case 4:
                        spawnPlayers.Add(playerPrefab1);
                        spawnPlayers.Add(playerPrefab2);
                        spawnPlayers.Add(playerPrefab3);
                        spawnPlayers.Add(playerPrefab4);
                        break;
                }


                for (int i = 0; i < StatHolder.HowManyPlayers; i++)
                {
                    if (playerSpawns.Count > 0 && spawnPlayers.Count > 0)
                    {
                        playerSpawns[Random.Range(0, playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
                    }
                }
                playersAlive = StatHolder.HowManyPlayers;
                RoundStart();
            }
        }
    }

    public void PlayerChecker()
    {
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:
                playersAlive -= 1;
                if (playersAlive == 1)
                {
                    RoundOver();
                }
                break;

            case StatHolder.Modes.TDM:
                if (redPlayers.Count == 0 && bluePlayers.Count > 0 || bluePlayers.Count == 0 && redPlayers.Count > 0)
                {
                    if (teamWon == false)
                    {
                        RoundOver();
                        teamWon = true;
                    }
                }
                break;
        }

    }

    public void NewGame()
    {
        if (StatHolder.HowManyPlayers == 0)
        {
            StatHolder.HowManyPlayers = 2;
        }
        StatHolder.WinsNeeded = 4;
        StatHolder.WhichSet = Random.Range(1, sceneLoader.setAmount + 1);
        while (StatHolder.usedSets.Contains(StatHolder.WhichSet))
        {
            StatHolder.WhichSet = Random.Range(1, sceneLoader.setAmount + 1);
        }
        StatHolder.usedSets.Add(StatHolder.WhichSet);
        audioScript.PlaySceneMusic(StatHolder.WhichSet);
        NewRound();
    }

    public void NewRound()
    {


        switch (StatHolder.WhichSet)
        {
            case 1:
                sceneLoader.NextSetScene(1);
                RoundStart();
                break;
            case 2:
                sceneLoader.NextSetScene(2);
                RoundStart();
                break;
            case 3:
                sceneLoader.NextSetScene(3);
                RoundStart();
                break;
            case 4:
                sceneLoader.NextSetScene(4);
                RoundStart();
                break;
            case 5:
                sceneLoader.NextSetScene(5);
                RoundStart();
                break;
            default:
                Debug.LogWarning("RoundManager NewRound() switch case default was chosen. Next map is randomized");
                sceneLoader.NewRandomScene();
                RoundStart();
                break;
        }

    }

    public void RoundStart()
    {
        //Run an animation showing the arena and how the obstacles function
        //Unfreeze everything
    }

    public void RoundOver()
    {
        audioScript.PlayRoundOverSound();

        //Freeze eveything or do some other kind of ending stuff. Maybe a cool animation?

        //Add a win to the player/team who won the round and announce that winner
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:

                switch (alivePlayers[0].name)
                {
                    case "Player1(Clone)":
                        StatHolder.Player1Wins += 1;
                        whoWonText.text = "Red player won the round";
                        whoWonText.color = Red;
                        break;
                    case "Player2(Clone)":
                        StatHolder.Player2Wins += 1;
                        whoWonText.text = "Blue player won the round";
                        whoWonText.color = Blue;
                        break;
                    case "Player3(Clone)":
                        StatHolder.Player3Wins += 1;
                        whoWonText.text = "Cyan player won the round";
                        whoWonText.color = Cyan;
                        break;
                    case "Player4(Clone)":
                        StatHolder.Player4Wins += 1;
                        whoWonText.text = "Yellow player won the round";
                        whoWonText.color = Yellow;
                        break;
                    default:
                        print("This should never happen");
                        break;
                }
                alivePlayers[0].GetComponent<PlayerHealth>().VFX_Win();
                ScoreAmount1[1].transform.parent.gameObject.SetActive(true);
                ScoreAmount2[1].transform.parent.gameObject.SetActive(true);
                ScoreAmount1[1].fillAmount = 0.2f + StatHolder.Player1Wins * 0.2f;

                ScoreAmount2[1].fillAmount = 0.2f + StatHolder.Player2Wins * 0.2f;
                ScoreAmount3[1].transform.parent.gameObject.SetActive(false);
                ScoreAmount4[1].transform.parent.gameObject.SetActive(false);
                if (StatHolder.HowManyPlayers > 2)
                {
                    ScoreAmount3[1].transform.parent.gameObject.SetActive(true);
                    ScoreAmount3[1].fillAmount = 0.2f + StatHolder.Player3Wins * 0.2f;

                    if (StatHolder.HowManyPlayers > 3)
                    {
                        ScoreAmount4[1].transform.parent.gameObject.SetActive(true);
                        ScoreAmount4[1].fillAmount = 0.2f + StatHolder.Player4Wins * 0.2f;
                    }
                }
                StatHolder.MostWins = Mathf.RoundToInt(Mathf.Max(StatHolder.Player1Wins, StatHolder.Player2Wins, StatHolder.Player3Wins, StatHolder.Player4Wins));
                ScoreAmount1[4].GetComponent<Image>().sprite = PlayerEmotions[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player1Wins), 0, 3)];
                ScoreAmount2[4].GetComponent<Image>().sprite = PlayerEmotions[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player2Wins), 0, 3) + 4];
                ScoreAmount3[4].GetComponent<Image>().sprite = PlayerEmotions[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player3Wins), 0, 3) + 8];
                ScoreAmount4[4].GetComponent<Image>().sprite = PlayerEmotions[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player4Wins), 0, 3) + 12];
                ScoreAmount1[3].GetComponent<Image>().sprite = PlayerTorsos[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player1Wins), 0, 3)];
                ScoreAmount2[3].GetComponent<Image>().sprite = PlayerTorsos[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player2Wins), 0, 3) + 4];
                ScoreAmount3[3].GetComponent<Image>().sprite = PlayerTorsos[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player3Wins), 0, 3) + 8];
                ScoreAmount4[3].GetComponent<Image>().sprite = PlayerTorsos[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.Player4Wins), 0, 3) + 12];
                break;

            case StatHolder.Modes.TDM:
                if (redPlayers.Count == 0)
                {
                    StatHolder.TeamBlueWins += 1;
                    foreach(GameObject Player in bluePlayers)
                    {
                        Player.GetComponent<PlayerHealth>().VFX_Win();
                    }
                    whoWonText.text = "Team Blue won the round";
                    whoWonText.color = Blue;
                }
                else if (bluePlayers.Count == 0)
                {
                    StatHolder.TeamRedWins += 1;
                    foreach (GameObject Player in redPlayers)
                    {
                        Player.GetComponent<PlayerHealth>().VFX_Win();
                    }
                    whoWonText.text = "Team Red won the round";
                    whoWonText.color = Red;
                }
                ScoreAmount1[1].transform.parent.gameObject.SetActive(false);
                ScoreAmount4[1].transform.parent.gameObject.SetActive(false);
                ScoreAmount2[1].fillAmount = 0.2f + StatHolder.TeamRedWins * 0.2f;
                ScoreAmount3[1].fillAmount = 0.2f + StatHolder.TeamBlueWins * 0.2f;

                StatHolder.MostWins = Mathf.RoundToInt(Mathf.Max(StatHolder.TeamRedWins, StatHolder.TeamBlueWins));
                ScoreAmount3[0].GetComponent<Image>().sprite = ScoreAmount2[0].GetComponent<Image>().sprite;
                ScoreAmount3[1].GetComponent<Image>().sprite = ScoreAmount2[1].GetComponent<Image>().sprite;
                ScoreAmount3[2].GetComponent<Image>().sprite = ScoreAmount2[2].GetComponent<Image>().sprite;
                ScoreAmount3[3].GetComponent<Image>().sprite = ScoreAmount2[3].GetComponent<Image>().sprite;
                ScoreAmount2[0].GetComponent<Image>().sprite = ScoreAmount1[0].GetComponent<Image>().sprite;
                ScoreAmount2[1].GetComponent<Image>().sprite = ScoreAmount1[1].GetComponent<Image>().sprite;
                ScoreAmount2[2].GetComponent<Image>().sprite = ScoreAmount1[2].GetComponent<Image>().sprite;
                ScoreAmount2[3].GetComponent<Image>().sprite = ScoreAmount1[3].GetComponent<Image>().sprite;

                ScoreAmount2[4].GetComponent<Image>().sprite = PlayerEmotions[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.TeamRedWins), 0, 3)];
                ScoreAmount3[4].GetComponent<Image>().sprite = PlayerEmotions[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.TeamBlueWins), 0, 3) + 4];
                ScoreAmount2[3].GetComponent<Image>().sprite = PlayerTorsos[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.TeamRedWins), 0, 3)];
                ScoreAmount3[3].GetComponent<Image>().sprite = PlayerTorsos[Mathf.Clamp(Mathf.RoundToInt(StatHolder.MostWins - StatHolder.TeamBlueWins), 0, 3) + 4];

                break;
        }

        ScoreAmount1[3].GetComponent<RectTransform>().localPosition = new Vector3(-300, ScoreAmount1[1].GetComponent<Image>().fillAmount * ScoreAmount1[1].GetComponent<RectTransform>().rect.height - 65 - ScoreAmount1[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount1[4].GetComponent<RectTransform>().localPosition = new Vector3(-300, ScoreAmount1[1].GetComponent<Image>().fillAmount * ScoreAmount1[1].GetComponent<RectTransform>().rect.height + 75 - ScoreAmount1[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount2[3].GetComponent<RectTransform>().localPosition = new Vector3(-100, ScoreAmount2[1].GetComponent<Image>().fillAmount * ScoreAmount2[1].GetComponent<RectTransform>().rect.height - 65 - ScoreAmount2[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount2[4].GetComponent<RectTransform>().localPosition = new Vector3(-100, ScoreAmount2[1].GetComponent<Image>().fillAmount * ScoreAmount2[1].GetComponent<RectTransform>().rect.height + 75 - ScoreAmount2[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount3[3].GetComponent<RectTransform>().localPosition = new Vector3(100, ScoreAmount3[1].GetComponent<Image>().fillAmount * ScoreAmount3[1].GetComponent<RectTransform>().rect.height - 65 - ScoreAmount3[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount3[4].GetComponent<RectTransform>().localPosition = new Vector3(100, ScoreAmount3[1].GetComponent<Image>().fillAmount * ScoreAmount3[1].GetComponent<RectTransform>().rect.height + 75 - ScoreAmount3[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount4[3].GetComponent<RectTransform>().localPosition = new Vector3(300, ScoreAmount4[1].GetComponent<Image>().fillAmount * ScoreAmount4[1].GetComponent<RectTransform>().rect.height - 65 - ScoreAmount4[1].GetComponent<Image>().fillAmount * 100, 0);
        ScoreAmount4[4].GetComponent<RectTransform>().localPosition = new Vector3(300, ScoreAmount4[1].GetComponent<Image>().fillAmount * ScoreAmount4[1].GetComponent<RectTransform>().rect.height + 75 - ScoreAmount4[1].GetComponent<Image>().fillAmount * 100, 0);



        //Check if anyone has enough points to win the game and announce the winner if they do
        if (StatHolder.Player1Wins >= StatHolder.WinsNeeded || StatHolder.Player2Wins >= StatHolder.WinsNeeded || StatHolder.Player3Wins >= StatHolder.WinsNeeded || StatHolder.Player4Wins >= StatHolder.WinsNeeded || StatHolder.TeamRedWins >= StatHolder.WinsNeeded || StatHolder.TeamBlueWins >= StatHolder.WinsNeeded)
        {
            //Game is over
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:

                    switch (alivePlayers[0].name)
                    {
                        case "Player1(Clone)":
                            whoWonText.text = "Red player won the game";
                            break;
                        case "Player2(Clone)":
                            whoWonText.text = "Blue player won the game";
                            break;
                        case "Player3(Clone)":
                            whoWonText.text = "Cyan player won the game";
                            break;
                        case "Player4(Clone)":
                            whoWonText.text = "Yellow player won the game";
                            break;
                        default:
                            print("This should never happen");
                            break;
                    }
                    break;

                case StatHolder.Modes.TDM:
                    if (redPlayers.Count == 0)
                    {
                        whoWonText.text = "Blue Team won the game";
                    }
                    else if (bluePlayers.Count == 0)
                    {
                        whoWonText.text = "Red Team won the game";
                    }
                    break;
            }
            StartCoroutine(BackToMenu());
        }
        //If no one has enough wins, start a new round
        else
        {
            StartCoroutine(NextRound());
        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(1.5f);
        roundWon.SetActive(true);
        yield return new WaitForSeconds(4.8f);
        ScoreAmount1[1].transform.parent.gameObject.SetActive(false);
        ScoreAmount2[1].transform.parent.gameObject.SetActive(false);
        ScoreAmount3[1].transform.parent.gameObject.SetActive(false);
        ScoreAmount4[1].transform.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        StatHolder.RoundNumber += 1;
        NewRound();
        yield return new WaitForSeconds(0.02f);
        roundWon.SetActive(false);
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(1.5f);
        roundWon.SetActive(true);
        yield return new WaitForSeconds(4.8f);
        ScoreAmount1[1].transform.parent.gameObject.SetActive(false);
        ScoreAmount2[1].transform.parent.gameObject.SetActive(false);
        ScoreAmount3[1].transform.parent.gameObject.SetActive(false);
        ScoreAmount4[1].transform.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        audioScript.StopPlayingSceneMusic();
        audioScript.PlaySceneMusic(0);
        sceneLoader.MenuScene();
        yield return new WaitForSeconds(0.02f);
        roundWon.SetActive(false);
    }

    IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(weaponSpawnTimer);

        int i = Random.Range(0, weaponSpawns.Count);
        weaponSpawns[i].GetComponent<WeaponSpawn>().CreateWeapon();
        StartCoroutine(SpawnWeapon());

    }
}
