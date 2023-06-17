using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int damage;

    public GameObject UI_Heart;
    public Transform HeartParent;

    List<GameObject> HeartsUI;

    private void Start()
    {
        HeartsUI = new List<GameObject>();
        health = maxHealth;
        UIHealthStarter();
    }
    public void AddDamage(int damage) {
        health -= damage;
        UIHealthReducer();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)){
            AddDamage(1);
        }
    }
    public void UIHealthStarter()
    {
        int a = (int) Mathf.Ceil(maxHealth / 3);

        for (int i = 0; i < a; i++) {

            GameObject HeartObject = Instantiate(UI_Heart, HeartParent);

            HeartsUI.Add(HeartObject);
        }

    }
    public void UIHealthReducer() {
        Debug.Log("YO");
        if (health % 3 == 0)
        {
            HeartsUI[HeartsUI.Count - 1].GetComponent<Animator>().SetTrigger("Crack-3");

            Destroy(HeartsUI[HeartsUI.Count - 1], 0.5f);
            HeartsUI.RemoveAt(HeartsUI.Count - 1);

        }
        else if (health % 3 == 1) {
            //Activate Animation Stage 2 Here
            HeartsUI[HeartsUI.Count - 1].GetComponent<Animator>().SetTrigger("Crack-2");
            //HeartsUI[HeartsUI.Count - 1].GetComponent<Image>().color = Color.green;
        }
        else if (health % 3 == 2)
        {
            //Activate Animation Stage 1 Here
            HeartsUI[HeartsUI.Count - 1].GetComponent<Animator>().SetTrigger("Crack-1");
            //HeartsUI[HeartsUI.Count - 1].GetComponent<Image>().color = Color.red;
        }
    }
}
