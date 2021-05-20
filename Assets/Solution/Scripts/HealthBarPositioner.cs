using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPositioner : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = Vector3.zero; //offset added to healthbar position

    private Enemy enemy;

    private RectTransform healthbar;


    private void Start()
    {
        healthbar = GetComponent<RectTransform>();
        enemy = GetComponentInChildren<HealthBarValue>().enemy;
    }

    void Update()
    {
        Vector3 finalPos = enemy.transform.position + offset;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, finalPos);
        
        healthbar.position = screenPoint;
    }

}
