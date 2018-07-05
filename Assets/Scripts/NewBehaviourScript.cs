using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public int spawnTime = 10;

    public bool independent;
    public bool startWeapon;
    public bool randomWeapon;
    public bool randomPosition;

    bool pickedUp;

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

    public List<Transform> weapons = new List<Transform>();
    List<int> chances = new List<int>();

    Transform spawnedWeapon;


    // Use this for initialization
    void Start()
    {
        chances.Add(FlailChanceWeight);
        chances.Add(GreatAxeChanceWeight);
        chances.Add(HammerChanceWeight);

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
    void Update()
    {
        RaycastHit hit;
        Ray spawnRay = new Ray(this.gameObject.transform.position, Vector3.down);
        Debug.DrawRay(this.gameObject.transform.position, Vector3.down * 1);

        if (!Physics.Raycast(spawnRay, out hit, 1) && randomPosition || hit.collider.tag != "Environment" && randomPosition)
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
    }


    public void CreateWeapon()
    {

        if (randomWeapon)
        {
            int all = 0;
            foreach (int chance in chances)
            {
                all += chance;
            }
            int i = Random.Range(0, all);
            int current = 0;

            for (int e = 0; e < chances.Count; e++)
            { 
                current += chances[e];
                if (i <= current)
                {
                    weaponToSpawn = (SPAWN_WEAPON)e;
                    break;
                }
            }

        }

        switch (weaponToSpawn)
        {

            case SPAWN_WEAPON.FLAIL:
                spawnedWeapon = Instantiate(weapons[0], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.GREATAXE:
                spawnedWeapon = Instantiate(weapons[1], this.gameObject.transform.position, Quaternion.AngleAxis(90, Vector3.right));

                break;

            case SPAWN_WEAPON.HAMMER:
                spawnedWeapon = Instantiate(weapons[2], this.gameObject.transform.position, Quaternion.identity);

                break;

        }

        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }

        pickedUp = false;

    }
}
