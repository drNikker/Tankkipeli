using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour
{

    public Transform goal;
    public List<GameObject> weapons;
    public List<GameObject> players;
    NavMeshAgent agent;
    AIHands[] Hands;
    float dist;
    bool weaponFindMode = false;
    float currentGoalDistance;
    RoundManager roundManager;
    void Start()
    {
        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        agent = GetComponent<NavMeshAgent>();
        Hands = GetComponentsInChildren<AIHands>();
        agent.updateRotation = true;
        StartCoroutine(LookForWeapon());

    }
    private void Update()
    {
        GoalFinder();
        GoalReached();

    }
    void GoalFinder()
    {

        if (weaponFindMode)
        {
            foreach (GameObject weapon in weapons)
            {
                float distance = Vector3.Distance(weapon.transform.position, transform.position);
                if (distance < currentGoalDistance)
                {
                    currentGoalDistance = distance;
                    goal = weapon.transform;
                    agent.destination = goal.position;
                }

            }
            if (goal.transform.root.tag != "Weapon")
            {
                weapons.Remove(goal.gameObject);
                goal = weapons[0].transform;
                agent.destination = goal.position;
            }

        }
        else
        {
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < currentGoalDistance)
                {
                    currentGoalDistance = distance;
                    goal = player.transform;
                    agent.destination = goal.position;
                }
            }
        }
    }

    void GoalReached()
    {
        dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.5f) //Arrived.
        {

            if (weaponFindMode)
            {
                if (!Hands[0].weaponInHand || !Hands[1].weaponInHand)
                {
                    if (!Hands[0].weaponInHand)
                    {
                        Hands[0].KeyPresses();
                    }
                    else if (!Hands[1].weaponInHand)
                    {
                        Hands[1].KeyPresses();
                    }
                }
                else if (Hands[0].weaponInHand && Hands[1].weaponInHand)
                {
                    LookForPlayers();
                }

            }
            //else
            //{
            //    LookForPlayers();
            //}
        }
        if (!weaponFindMode)
        {
            LookForPlayers();
        }

        }

    IEnumerator LookForWeapon()
    {
        yield return new WaitForSeconds(0.2f);
        weapons.Clear();
        weaponFindMode = true;
        weapons.AddRange(GameObject.FindGameObjectsWithTag("Weapon"));
        //foreach(GameObject weapon in weapons)
        //{
        //    if(weapon.transform.root.tag != "Weapon")
        //    {
        //        weapons.Remove(weapon);
        //    }
        //}
        weapons.Remove(Hands[0].weapon);
        print("jälkeen " + weapons.Count);
        currentGoalDistance = Vector3.Distance(weapons[0].transform.position, transform.position);
        print(weapons[0]);
        goal = weapons[0].transform;
        agent.destination = goal.position;
    }
    void LookForPlayers()
    {
        players.Clear();
        weaponFindMode = false;
        players.AddRange(roundManager.alivePlayers);
        players.Remove(transform.gameObject);
        //currentGoalDistance = Vector3.Distance(players[0].transform.position, transform.position);
        if (players != null)
        {
            goal = GetClosestPlayer(players).transform;
            agent.destination = goal.position;
        }
    }


    GameObject GetClosestPlayer(List<GameObject> targetPlayers)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in targetPlayers)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}