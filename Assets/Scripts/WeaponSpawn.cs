using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSpawn : MonoBehaviour {

    public int spawnTime=10;

    public bool randomWeapon;
    public bool randomPosition;

    [Range(0, 100)] public int FlailChanceWeight;
    [Range(0, 100)] public int GreatAxeChanceWeight;
    [Range(0, 100)] public int HammerChanceWeight;



    public SPAWN_WEAPON weaponToSpawn;

    public Transform GreatAxe;
    public Transform Hammer;
    public Transform Flail;

    

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnWeapon());
    }



    // Update is called once per frame
    void Update ()
    {
		
	}

    IEnumerator SpawnWeapon()
    {
        if (randomWeapon)
        {

            int i = Random.Range(0, FlailChanceWeight + GreatAxeChanceWeight + HammerChanceWeight);

            if (i > 0 && i < FlailChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.FLAIL;
            }
            else if (i > FlailChanceWeight && i < FlailChanceWeight + GreatAxeChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.GREATAXE; ;
            }
            else if (i > FlailChanceWeight + GreatAxeChanceWeight && i < FlailChanceWeight + GreatAxeChanceWeight + HammerChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.HAMMER;
            }
            else
            {
                Debug.Log("kaikki 0");
            }

        }
        yield return new WaitForSeconds(spawnTime);
        CreateWeapon();
    }


    public enum SPAWN_WEAPON
    {
        FLAIL,
        GREATAXE,
        HAMMER,
    }


    public void CreateWeapon()
    {

        switch (weaponToSpawn)
        {
            case SPAWN_WEAPON.GREATAXE:
                Instantiate(GreatAxe, this.gameObject.transform.position, Quaternion.AngleAxis(90, Vector3.right));

                break;

            case SPAWN_WEAPON.HAMMER:
                Instantiate(Hammer, this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.FLAIL:
                Instantiate(Flail, this.gameObject.transform.position, Quaternion.identity);

                break;
        }
        StartCoroutine(SpawnWeapon());

    }


}
