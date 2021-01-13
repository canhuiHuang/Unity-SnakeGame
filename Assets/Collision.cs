using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public Action foodTaken;
    public Action<GameObject> bodyHit;

    GameManagerScript manager;

    void OnTriggerEnter(Collider triggerCollider)
    {
        //Debug purpose
        print("XDDD");
        print(triggerCollider.gameObject.name);

        //
        if (triggerCollider.gameObject.tag == "food")
        {
            Destroy(triggerCollider.gameObject);
            if (foodTaken != null)
                foodTaken();
        }

        if (triggerCollider.gameObject.tag == "bodyType")
        {
            if (bodyHit != null)
                bodyHit(triggerCollider.gameObject);
        }

    }
}
