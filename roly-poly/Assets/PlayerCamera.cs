using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float cameraFollowSpeed;
    private float cameraZPos;

    private bool isZoomOut = false;
    private float tempCameraZPos;

    void Awake()
    {
        PlayerController.PlayerGotEgg += PlayerGotEggHandler;
    }

    void Start()
    {
        cameraZPos = transform.position.z;
        tempCameraZPos = cameraZPos - 15;
    }
    void FixedUpdate()
    {
        if (isZoomOut)
        {
            transform.position = (Vector3)Vector2.Lerp(transform.position, playerTransform.position, Time.deltaTime * cameraFollowSpeed) + Vector3.forward * tempCameraZPos;

        }
        else
        {
            transform.position = (Vector3)Vector2.Lerp(transform.position, playerTransform.position, Time.deltaTime * cameraFollowSpeed) + Vector3.forward * cameraZPos;

        }
    }

    private void PlayerGotEggHandler(PlayerController p)
    {
        StartCoroutine("ZoomOutForSeconds");
    }

    IEnumerator ZoomOutForSeconds()
    {
        isZoomOut = true;

        yield return new WaitForSeconds(2f);
        isZoomOut = false;


    }

    void OnDestroy()
    {
        PlayerController.PlayerGotEgg -= PlayerGotEggHandler;
    }

}
