using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectDamage : MonoBehaviour
{

    PlayerHealth health;
    Rigidbody rb;
    Rigidbody tankBase;

    public float baseDamage = 16;
    public float cooldownTime = 1;
    public float knockback = 400000;
    public ParticleSystem VFX;
    int finalDamageVFX;
    private float startCooldown;

    float cooldown;
    float finalDamage;
    private bool canDamage = true;
    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;

    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        VFX = GetComponent<ParticleSystem>();
        startCooldown = Time.time + 2;
        Destroy(gameObject, 45);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time && canDamage)
        {
            health = FindHP(collision);
            tankBase = FindTank(collision);

            finalDamage = baseDamage * (rb.mass / 200) * (rb.velocity.magnitude / 5);       //Deal damage based on the damage values and the force of the impact
            //Debug.Log(finalDamage + " THAT'S A LOTTA DAMAGE!");
            health.TakeDamage(finalDamage);                                  //Tells how much damage to deal
            Vector3 dir = collision.transform.position - transform.position;
            dir.y = 0;

            tankBase.AddForce(dir.normalized * knockback);
            cooldown = Time.time + cooldownTime;                             //Puts the weapon on cooldown to avoid spam

            finalDamageVFX = Mathf.RoundToInt(finalDamage);
            VFX.startLifetime = (0.05f * finalDamageVFX);
            VFX.Emit(5 * finalDamageVFX);
            playSound();
        } else if (rb.velocity.magnitude <= 1 && Time.time > startCooldown)
        {
            canDamage = false;
            //Debug.Log("Physics object can't damage anymore");
        }

    }

    static PlayerHealth FindHP(Collision col)                  //Finds the players hp
    {
        PlayerHealth hp = col.gameObject.GetComponentInParent<PlayerHealth>();
        Transform parentOb = col.gameObject.transform;

        while (parentOb != null)
        {
            hp = parentOb.GetComponent<PlayerHealth>();

            if (hp != null)
            {
                return hp;
            }
            parentOb = parentOb.transform.parent;
        }

        Debug.LogWarning("Bodypart took a hit, but player health was not found");
        return null;
    }

    static Rigidbody FindTank(Collision col)                  //Finds the players tank
    {
        Transform parentOb = col.gameObject.transform;

        while (parentOb != null)
        {
            if (parentOb.tag == "Player")
            {
                Rigidbody rig = parentOb.GetComponent<Rigidbody>();
                return rig;
            }
            parentOb = parentOb.transform.parent;
        }

        Debug.LogWarning("Bodypart took a hit, but player tankbase was not found");
        return null;
    }
    private void playSound()
    {
        currentAudioClip = audioScript.hazardAudioList[5];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
