using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Enemy enemy;

    private void Update()
    {
        //raycasting on enemy object to detect if it was clicked
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit; 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                enemy = hit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.DamageMultiDelegate(10);
                }
            }
        }
    }
}
