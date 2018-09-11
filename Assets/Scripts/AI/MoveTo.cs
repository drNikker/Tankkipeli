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
        agent.updateRotation = true;
        StartCoroutine(LookForWeapon());
    }
    private void Update()
    {
        test = agent.velocity;
        if(transform.position.x > transform.parent.position.x +0.1 || transform.position.x < transform.parent.position.x - 0.1)
        {
            transform.position = transform.parent.position;
        }
        if (transform.position.z > transform.parent.position.z + 0.1 || transform.position.z < transform.parent.position.z - 0.1)
        {
            transform.position = transform.parent.position;
        }
    }

    IEnumerator LookForWeapon()
    {
        yield return new WaitForSeconds(0.2f);
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        goal = weapons[1].transform;
        agent.destination = goal.position;
    }
}