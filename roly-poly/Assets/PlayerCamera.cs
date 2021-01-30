using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float cameraFollowSpeed;
    private float cameraZPos;

    void Start()
    {
        cameraZPos = transform.position.z;
    }
    void FixedUpdate()
    {
        transform.position = (Vector3)Vector2.Lerp(transform.position, playerTransform.position, Time.deltaTime * cameraFollowSpeed) + Vector3.forward * cameraZPos;
    }


}
