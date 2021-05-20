using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthBarValue : MonoBehaviour
{
    public Enemy enemy;
    public Image healthValueImg;

    [SerializeField]
    float randomizeFactor = 0.4f; // randomize position of PopUp for weight * randomizeFactor and height * randomizeFactor

    void Start()
    {
        healthValueImg = GetComponent<Image>();

        enemy.eventHealthValue.AddListener(HealthUpdate);
        enemy.eventPopUp.AddListener(PopUpCreate);

        HealthUpdate();
    }

    //Updating health value 
    public void HealthUpdate()
    {
        healthValueImg.fillAmount = (float)enemy.health / 100.0f;

        if (healthValueImg.fillAmount <= 0)
        {
            Destroy(transform.parent.gameObject);
        }

        GetComponentInChildren<TextMeshProUGUI>().text = healthValueImg.fillAmount * 100 + "/100";
    }

    //Randomize position of healthbar
    Vector2 RandomizePosition()
    {
        Vector2 result = new Vector2(0, 0);

        Rect hpBar = GetComponent<RectTransform>().rect;

        float weight = hpBar.width;
        float height = hpBar.height;

        float textFrame = 50.0f;

        while ((result.x < weight / 2.0f + textFrame 
            && result.x > -weight / 2.0f - textFrame) 
            && (result.y < height / 2.0f + textFrame 
            && result.y > -height / 2.0f - textFrame))
        {
            result = new Vector2(Random.Range(-weight * randomizeFactor, weight * randomizeFactor), Random.Range(-height * randomizeFactor, height * randomizeFactor));
        }

        return result;
    }

    //Creating PopUp with damage value
    public void PopUpCreate(int dmgValue)
    {
        Vector2 position = RandomizePosition();

        GameObject popUp = Instantiate(Resources.Load("PopUp") as GameObject, transform);

        popUp.GetComponent<TextMeshProUGUI>().text = dmgValue.ToString();
        popUp.transform.localPosition = position;

        StartCoroutine(DestroyPopUp(popUp));
    }

    //Destroying PopUp with damage value
    IEnumerator DestroyPopUp(GameObject popUp)
    {
        yield return new WaitForSecondsRealtime(1.0f);

        Destroy(popUp);
    }
}
