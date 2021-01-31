using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLava : MonoBehaviour
{
    public Transform endPoint;

    public GameObject lava;

    public bool isActive = false;

    public float speed;


    void Awake()
    {
        lava.SetActive(false);
    }
    void Update()
    {
        if (!isActive)
            return;
        if (lava.transform.position.y >= endPoint.transform.position.y)
        {
            isActive = false;
        }
        else
        {
            lava.transform.position = new Vector3(lava.transform.position.x, lava.transform.position.y + speed * Time.deltaTime, lava.transform.position.z);
        }
    }

    public void StartRising()
    {
        isActive = true;
        lava.SetActive(true);
    }
}
