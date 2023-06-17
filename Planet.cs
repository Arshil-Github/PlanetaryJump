using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update

    public float rotationSpeed;
    public DistanceJoint2D distanceJoint;

    float originalDistance;

    private void Awake()
    {
        distanceJoint = gameObject.GetComponent<DistanceJoint2D>();
    }
    void Start()
    {
        originalDistance = (transform.localScale.x / 2f) + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        distanceJoint.distance = originalDistance;
    }
    public void SetJoint(Rigidbody2D target_rb)
    {
        distanceJoint.connectedBody = target_rb;
    }
    public void DestroyJoint() {
        distanceJoint.connectedBody = null;
    } 
}
