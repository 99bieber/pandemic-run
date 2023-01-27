using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTest : MonoBehaviour
{
    public Transform objectToFollow;

    public float speed = 2f;
    Vector3 offset;

    void Start() {
        offset = transform.position - objectToFollow.position;    
    }

    void Update () {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, 4.15f, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
        position.z = offset.z + objectToFollow.position.z;


        this.transform.position = position;
    }
}
