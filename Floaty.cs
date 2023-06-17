using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty : MonoBehaviour
{
    public List<Transform> positionsItems;
    public float remainTime = 5f;
    public float speed;

    List<Vector3> positions;
    Vector3 finalLoc;
    bool startMove = false;
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Vector3>();

        for (int i = 0; i < positionsItems.Count - 1; i++) {
            positions.Insert(i, positionsItems[i].position);
        }

        positions.Insert(0, transform.position);
        StartCoroutine(MoveToDifferentLoc());
    }

    IEnumerator MoveToDifferentLoc() {

        yield return new WaitForSeconds(remainTime);

        if (finalLoc != positions[positions.Count - 1])
        {
            finalLoc = positions[positions.IndexOf(finalLoc) + 1];
        }
        else {
            finalLoc = positions[0];
        }
        startMove = true;
        //transform.position = finalLoc;
        


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (startMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, finalLoc, speed * Time.deltaTime);
            if (transform.position == finalLoc) {
                startMove = false;
                StartCoroutine(MoveToDifferentLoc());
            }
        }
    }
}
