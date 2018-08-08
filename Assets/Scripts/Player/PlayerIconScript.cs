using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconScript : MonoBehaviour
{
    [SerializeField] public bool showNoUpdate;

    public GameObject PlayerNoSprite;
    public GameObject spawnedPNo;
    
    public float yOffset = 0.75f;
    private float velocity;
    // Use this for initialization
    void Start()
    {
        //Spawns and hides player number icon
        spawnedPNo = Instantiate(PlayerNoSprite, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), transform.rotation);
        spawnedPNo.transform.parent = gameObject.transform;

        //spawnedPNo.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(ShowPlayerIcons(2));
    }

    // Update is called once per frame
    void Update()
    {
        if (showNoUpdate)
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, 1, ref velocity, 10f * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            spawnedPNo.transform.LookAt(-Camera.main.transform.position, Vector3.up);
            spawnedPNo.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
        }

        if (!showNoUpdate)
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, 0, ref velocity, 10f * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void ShowPIcons()
    {
        showNoUpdate = true;
    }

    public void HidePIcons()
    {
        showNoUpdate = false;
    }

    IEnumerator ShowPlayerIcons(int secondsToWait)
    {
        showNoUpdate = true;
        yield return new WaitForSeconds(secondsToWait);
        showNoUpdate = false;
    }
}
