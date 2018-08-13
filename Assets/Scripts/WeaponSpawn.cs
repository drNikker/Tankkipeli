using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{

    public int spawnTime = 10;

    public bool independent;
    public bool startWeapon;
    public bool randomWeapon;
    public bool randomPosition;
    public bool showGizmos;

    bool pickedUp;
    bool generatingNewWeapon = false;

    [Range(0, 100)] public int FlailChanceWeight0;
    [Range(0, 100)] public int GreatAxeChanceWeight1;
    [Range(0, 100)] public int HammerChanceWeight2;
    [Range(0, 100)] public int GladiatorChanceWeight3;
    [Range(0, 100)] public int ShieldChanceWeight4;
    [Range(0, 100)] public int CherryChanceWeight5;
    [Range(0, 100)] public int GuitarChanceWeight6;
    [Range(0, 100)] public int NunchucksChanceWeight7;
    [Range(0, 100)] public int TrophyChanceWeight8;
    [Range(0, 100)] public int LadleChanceWeight9;
    [Range(0, 100)] public int HockeyChanceWeight10;
    [Range(0, 100)] public int FistChanceWeight11;

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
        chances.Add(FlailChanceWeight0);
        chances.Add(GreatAxeChanceWeight1);
        chances.Add(HammerChanceWeight2);
        chances.Add(GladiatorChanceWeight3);
        chances.Add(ShieldChanceWeight4);
        chances.Add(CherryChanceWeight5);
        chances.Add(GuitarChanceWeight6);
        chances.Add(NunchucksChanceWeight7);
        chances.Add(TrophyChanceWeight8);
        chances.Add(LadleChanceWeight9);
        chances.Add(HockeyChanceWeight10);
        chances.Add(FistChanceWeight11);

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

        if ((!Physics.Raycast(spawnRay, out hit, 10000) && randomPosition) || (hit.collider.tag != "Environment" && randomPosition))
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));

        }
        if (spawnedWeapon != null)
        {
            if (spawnedWeapon.GetComponentInChildren<Weapon>().currentWeaponState == Weapon.WEAPON_STATE.WIELDED && pickedUp == false)
            {
                pickedUp = true;
                if (independent)
                {
                    StartCoroutine(SpawnWeapon());
                }
            }
        }
        else if (spawnedWeapon == null && generatingNewWeapon == false)
        {
            StartCoroutine(SpawnWeapon());
            generatingNewWeapon = true;
        }
    }

    IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(spawnTime);
        CreateWeapon();
        generatingNewWeapon = false;
    }


    public enum SPAWN_WEAPON
    {
        FLAIL,
        GREATAXE,
        HAMMER,
        GLADIATOR,
        SHIELD,
        CHERRY,
        GUITAR,
        NUNCHUCKS,
        TROPHY,
        LADLE,
        HOCKEY,
        FIST
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

            case SPAWN_WEAPON.GLADIATOR:
                spawnedWeapon = Instantiate(weapons[3], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.SHIELD:
                spawnedWeapon = Instantiate(weapons[4], this.gameObject.transform.position, Quaternion.identity);

                break;
            case SPAWN_WEAPON.CHERRY:
                spawnedWeapon = Instantiate(weapons[5], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.GUITAR:
                spawnedWeapon = Instantiate(weapons[6], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.NUNCHUCKS:
                spawnedWeapon = Instantiate(weapons[7], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.TROPHY:
                spawnedWeapon = Instantiate(weapons[8], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.LADLE:
                spawnedWeapon = Instantiate(weapons[9], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.HOCKEY:
                spawnedWeapon = Instantiate(weapons[10], this.gameObject.transform.position, Quaternion.identity);

                break;

            case SPAWN_WEAPON.FIST:
                spawnedWeapon = Instantiate(weapons[11], this.gameObject.transform.position, Quaternion.identity);

                break;
        }

        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), Random.Range(YPositionLowerLimit, YPositionUpperLimit), Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }

        pickedUp = false;



    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
            Gizmos.DrawWireCube(new Vector3((XPositionLowerLimit + XPositionUpperLimit) / 2, 0, (ZPositionLowerLimit + ZPositionUpperLimit) / 2), new Vector3(XPositionUpperLimit - XPositionLowerLimit, 0, ZPositionUpperLimit - ZPositionLowerLimit));
    }
}
