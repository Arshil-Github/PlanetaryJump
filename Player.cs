using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Values")]
    public float speed;
    public float maxSpeed;
    public float holdSpeed;
    public float upSpeed;
    public float gravity;
    public int attackCharges;
    public bool stuckToPlanet = true;

    [Header("References")]
    public Transform directionGuide;
    public GameObject chargeIndicator;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject pf_landEffect;

    public Transform indicatorsGrid_UI;
    public GameObject pf_individualCharge_UI;//UI

    [Header("PlayerHealth Stuff")]
    public PlayerHealth ph;

    Vector2 towardsCentre;
    Transform parent;
    Collider2D current;
    int currentCharge = 0;
    bool isAttackCharges = false;
    List<GameObject> chargesInstantiated;
    // Start is called before the first frame update
    void Start()
    {
        changePlanet();
        parent = transform.parent;

        directionGuide.gameObject.SetActive(false);

        chargesInstantiated = new List<GameObject>();
    }
    bool hold = true;
    // Update is called once per frame
    void Update()
    {
        #region Movement

        Vector3 diff = -towardsCentre;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        if (stuckToPlanet && hold == true)
        {
            towardsCentre = transform.parent.Find("centre").position - transform.position;
            //rb.AddForce(towardsCentre * (gravity + speed) * Time.deltaTime);

            current = parent.GetComponent<Collider2D>();

            rb.velocity = transform.right.normalized * speed;
            rb.AddForce(towardsCentre * gravity);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else
        {
            rb.velocity = transform.up.normalized * Mathf.Abs(upSpeed);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            hold = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            onHoldStart();
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.constraints = RigidbodyConstraints2D.None;
            towardsCentre = transform.parent.Find("centre").position - transform.position;

            rb.velocity = transform.right * holdSpeed;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            onHoldEnd();
            StartCoroutine(delayTriggerOff());
            transform.parent = null;

            currentCharge = currentCharge + 1;

            GameObject tempCharge = Instantiate(pf_individualCharge_UI, indicatorsGrid_UI);
            chargesInstantiated.Add(tempCharge);

            if (currentCharge == attackCharges + 1) {
                isAttackCharges = true;
                currentCharge = 0;
                anim.SetTrigger("StopCharge");

                foreach (GameObject g in chargesInstantiated) {
                    Destroy(g);
                }

            }
            
            parent.GetComponent<Planet>().DestroyJoint();

            rb.velocity = Vector2.zero;

        }
        #endregion
    }

    #region MovementFunctions
    IEnumerator delayTriggerOff() {

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.01f);
        stuckToPlanet = false;
        current.isTrigger = true;

    }
    public void changePlanet()
    {
        towardsCentre = transform.parent.Find("centre").position - transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent = collision.transform;
        parent = transform.parent;

        speed = gameObject.GetComponentInParent<Planet>().rotationSpeed;
        maxSpeed = speed;

        rb.velocity = Vector2.zero;
        gameObject.GetComponentInParent<Planet>().SetJoint(rb);

        current.isTrigger = false;


        GameObject landEffect = Instantiate(pf_landEffect, transform.position, Quaternion.identity);

        isAttackCharges = false;

        chargeIndicator.SetActive(false);
        if (currentCharge == attackCharges)
        {
            anim.SetTrigger("Charge");
            chargeIndicator.SetActive(true);
        }

        stuckToPlanet = true;
        hold = true;
    }
    #endregion

    bool invulnerable = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {

            if (isAttackCharges)
            {
                //Attack Stuff
            }
            else
            {
                if (!invulnerable) {
                    ph.AddDamage(collision.gameObject.GetComponent<EnemyHealth>().damage);
                    //Add Damage

                    StartCoroutine(MakeVulnerable());
                }
            }

        }
    }
    IEnumerator MakeVulnerable() {
        invulnerable = true;
        yield return new WaitForSeconds(1f);
        invulnerable = false;
    }
    public void onHoldStart()
    {
        directionGuide.gameObject.SetActive(true);
    }
    public void onHoldEnd()
    {
        directionGuide.gameObject.SetActive(false);
    }
}
