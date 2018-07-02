using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSpawn : MonoBehaviour {

    public int spawnTime=10;

    public bool independent;
    public bool startWeapon;
    public bool randomWeapon;
    public bool randomPosition;

    public GameObject WeaponSpawnEffect;

    [Range(0, 100)] public int FlailChanceWeight;
    [Range(0, 100)] public int GreatAxeChanceWeight;
    [Range(0, 100)] public int HammerChanceWeight;

    public int XPositionLowerLimit;
    public int XPositionUpperLimit;
    public float YPositionLowerLimit;
    public float YPositionUpperLimit;
    public int ZPositionLowerLimit;
    public int ZPositionUpperLimit;


    public SPAWN_WEAPON weaponToSpawn;

    public GameObject greatAxe;
    public GameObject hammer;
    public GameObject flail;

    

    // Use this for initialization
    void Start ()
    {
        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }

        if (independent)
        {
            if (startWeapon)
            {
                CreateWeapon();
            }
            else
            {
                StartCoroutine(SpawnWeapon());
            }
        }

    }



    // Update is called once per frame
    void Update ()
    {
        RaycastHit hit;
        Ray spawnRay = new Ray(this.gameObject.transform.position, Vector3.down);
        Debug.DrawRay(this.gameObject.transform.position, Vector3.down * 1);

        if (!Physics.Raycast(spawnRay, out hit, 1) && randomPosition || hit.collider.tag != "Environment" && randomPosition)
        {

                this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
            
        }
    }

    IEnumerator SpawnWeapon()
    {
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
        switch (weaponToSpawn)
        {


            case SPAWN_WEAPON.GREATAXE:
            
               Instantiate(greatAxe, this.gameObject.transform.position, Quaternion.AngleAxis(90, Vector3.right));
                

                break;

            case SPAWN_WEAPON.HAMMER:

                Instantiate(hammer, this.gameObject.transform.position, Quaternion.identity);
              

                break;

            case SPAWN_WEAPON.FLAIL:
               
                Instantiate(flail, this.gameObject.transform.position, Quaternion.identity);
                
                break;
        }
        if (independent)
        {
            StartCoroutine(SpawnWeapon());
        }

        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }
    }


}
