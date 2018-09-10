using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform goal;
    public GameObject[] weapons;
    public Vector3 test;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(LookForWeapon());
    }
    private void Update()
    {
        test = agent.velocity;
    }

    IEnumerator LookForWeapon()
    {
        yield return new WaitForSeconds(0.2f);
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        goal = weapons[1].transform;
        agent.destination = goal.position;
    }
}