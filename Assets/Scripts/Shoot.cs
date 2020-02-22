using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private float _weaponRange = 100.0f;
    public Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("Shoot : Main Camera is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");
            RaycastHit objHit;
            bool hasHitSomething = false;

            hasHitSomething = Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out objHit, _weaponRange);

            if (hasHitSomething)
            {
                Debug.Log("We've hit : " + objHit.transform.name);
            }
        }
    }
}
