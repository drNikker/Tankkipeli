using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSpawn : MonoBehaviour {

    public int spawnTime=10;

    public bool independent;
    public bool startWeapon;
    public bool randomWeapon;
    public bool randomPosition;

    bool pickedUp;
    

    [Range(0, 100)] public int FlailChanceWeight;
    [Range(0, 100)] public int GreatAxeChanceWeight;
    [Range(0, 100)] public int HammerChanceWeight;
    [Range(0, 100)] public int StickChanceWeight;

    public int XPositionLowerLimit;
    public int XPositionUpperLimit;
    public float YPositionLowerLimit;
    public float YPositionUpperLimit;
    public int ZPositionLowerLimit;
    public int ZPositionUpperLimit;


    public SPAWN_WEAPON weaponToSpawn;

    public Transform GreatAxe;
    public Transform Hammer;
    public Transform Flail;
    public Transform Stick;

    Transform spawnedWeapon;
    

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

        if (!Physics.Raycast(spawnRay, out hit, 10000) && randomPosition || hit.collider.tag != "Environment" && randomPosition)
        {

                this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
            
        }
        if (spawnedWeapon.GetComponentInChildren<Weapon>().currentWeaponState == Weapon.WEAPON_STATE.WIELDED && pickedUp == false)
        {
            pickedUp = true;
            if (independent)
            {
                StartCoroutine(SpawnWeapon());
            }
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
        STICK,
    }


    public void CreateWeapon()
    {


        if (randomWeapon)
        {

            int i = Random.Range(0, FlailChanceWeight + GreatAxeChanceWeight + HammerChanceWeight + StickChanceWeight);

            if (i > 0 && i < FlailChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.FLAIL;
            }
            else if (i > FlailChanceWeight && i < FlailChanceWeight + GreatAxeChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.GREATAXE;
            }
            else if (i > FlailChanceWeight + GreatAxeChanceWeight && i < FlailChanceWeight + GreatAxeChanceWeight + HammerChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.HAMMER;
            }
            else if (i > FlailChanceWeight + GreatAxeChanceWeight + HammerChanceWeight && i < FlailChanceWeight + GreatAxeChanceWeight + HammerChanceWeight + StickChanceWeight)
            {
                weaponToSpawn = SPAWN_WEAPON.STICK;
            }
            else
            {
                Debug.Log("kaikki 0");
            }

        }
        switch (weaponToSpawn)
        {


            case SPAWN_WEAPON.GREATAXE:
                spawnedWeapon = Instantiate(GreatAxe, this.gameObject.transform.position, Quaternion.AngleAxis(90, Vector3.right));


                break;

            case SPAWN_WEAPON.HAMMER:
                spawnedWeapon = Instantiate(Hammer, this.gameObject.transform.position, Quaternion.identity);


                break;

            case SPAWN_WEAPON.FLAIL:
                spawnedWeapon = Instantiate(Flail, this.gameObject.transform.position, Quaternion.identity);

                break;
            case SPAWN_WEAPON.STICK:
                spawnedWeapon = Instantiate(Stick, this.gameObject.transform.position, Quaternion.identity);

                break;
        }

        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }
        pickedUp = false;
    }


}
