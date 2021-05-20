using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public int health;

    public delegate void Delegate(int value);
    public Delegate DamageMultiDelegate; //multicast delegate for dealing damage and updating HP bar, popup show and color change
    public UnityEvent eventHealthValue;

    [Serializable]
    public class InteractableObjectEvent : UnityEvent<int> { }

    public InteractableObjectEvent eventPopUp;

    [SerializeField]
    private GameObject _canvas;

    public GameObject canvas
    { 
        get => _canvas; 
        private set { _canvas = value; } 
    }

    private GameObject EnemyHealthBar;
    private Coroutine colorChange;

    void Start()
    {
        if (canvas == null)
        {
            SetupCanvas();
        }

        health = 100;
        EnemyHealthBar = Instantiate(Resources.Load("EnemyHealthBar") as GameObject, canvas.transform);
        EnemyHealthBar.GetComponentInChildren<HealthBarValue>().enemy = this;

        if (EnemyHealthBar == null)
        {
            Debug.LogError("Prefab not exist in Resources folder");
        }

        if (eventHealthValue == null)
        {
            eventHealthValue = new UnityEvent();
        }

        SubscribeEvents();
    }

    private void SetupCanvas()
    {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
            canvas.name = "Canvas";
            canvas.layer = 5;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }

    private void SubscribeEvents()
    {
        DamageMultiDelegate += InvokeHPEvent;
        DamageMultiDelegate += InvokePopUpEvent;
        DamageMultiDelegate += ChangeColor;
    }

    private void UnsubscribeEvent()
    {
        DamageMultiDelegate -= InvokeHPEvent;
        DamageMultiDelegate -= InvokePopUpEvent;
        DamageMultiDelegate -= ChangeColor;
    }

    //invoking event that changes HP value (dealing damage)
    private void InvokeHPEvent(int value)
    {
        health -= 10;
        eventHealthValue.Invoke();
        healthCheck();
    }

    //invoking event that shows PopUp with damage value
    private void InvokePopUpEvent(int value)
    {
        eventPopUp.Invoke(value);
    }

    //Changing color of an enemy when damage is dealt
    private void ChangeColor(int time)
    {
        GetComponent<Renderer>().material.color = Color.red;

        if (colorChange != null)
        {
            StopCoroutine(colorChange);
        }

        colorChange = StartCoroutine(fadeToWhite(time));
    }

    //fading enemy color from red to white
    IEnumerator fadeToWhite(int time)
    {
        float deltaTime = 0;

        yield return new WaitForSecondsRealtime(1.0f);

        while (deltaTime < time)
        {
            GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.white, deltaTime);

            deltaTime += Time.deltaTime;

            yield return null;
        }

        colorChange = null;
    }

    //Stoping color changing coroutine
    private void OnDestroy()
    {
        if (colorChange != null)
            StopCoroutine(colorChange);

        UnsubscribeEvent();
    }

    //destroying enemy object when health is under 0 or exactly 0
    private void healthCheck()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}
