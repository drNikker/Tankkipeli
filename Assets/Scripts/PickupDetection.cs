using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDetection : MonoBehaviour {

    HandControls[] hands;

    private void Start()
    {
        hands = GetComponentsInChildren<HandControls>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            foreach (HandControls hand in hands)
            {
                hand.WeaponInReach(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            foreach (HandControls hand in hands)
            {
                hand.WeaponOutOfReach();
            }
        }
    }
}
