using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] Cars;
    public float MaxTime, MinTime;
    public float Ypos;
    bool Spawn = false;
    public bool Inside = false;

    void Update()
    {
        if (!Spawn && Inside)
        {
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*1.5f,0,10));
            Instantiate(Cars[Random.Range(0, Cars.Length)],screenPos+ new Vector3(0,Ypos),Quaternion.identity);
            StartCoroutine(SpawnColdown());
        }
    }
    private IEnumerator SpawnColdown()
    {
        Spawn = true;
        yield return new WaitForSeconds(Random.Range(MinTime, MaxTime));
        Spawn = false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.name =="Player")
        {
            Inside = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform.name =="Player")
        {
            Inside = false;
            
        }
    }
}


