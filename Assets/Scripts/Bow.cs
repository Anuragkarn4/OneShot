using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject ArrowPrefab;

    [Header("Dynamic")] 
    public GameObject ShootPoint;
    public Vector3 shootPos;

    void Awake() {
        Transform shootPointTrans = transform.Find("ShootPoint");

        ShootPoint = shootPointTrans.gameObject;

        ShootPoint.SetActive( false );
    }


    void OnMouseEnter() {
        ShootPoint.SetActive( true );
    }



    void OnMouseExit() {
        ShootPoint.SetActive( false );

    }


}
