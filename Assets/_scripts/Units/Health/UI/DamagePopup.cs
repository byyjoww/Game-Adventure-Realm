using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public enum DamagePopupType { Damage = 0, Heal = 1, Blocked = 2, Critical = 3 };
    private TextMeshPro textMesh;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static DamagePopup Create(GameObject prefab, Vector3 position, int damageAmount, DamagePopupType damageType)
    {
        Transform damagePopupTransform = Instantiate(prefab, position, Quaternion.identity).transform;
        damagePopupTransform.LookAt(2 * damagePopupTransform.transform.position - Camera.main.transform.position);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, damageType);

        return damagePopup;
    }

    private static int sortingOrder;
    private const float DISSAPEAR_TIMER_MAX = 1f;    
    private float disappearTimer;    
    private Vector3 moveVector;

    private void SetPopup(string text, int fontSize, Color color)
    {
        textMesh.SetText(text);
        textMesh.fontSize = fontSize;
        textColor = color;
    }

    public void Setup(int popupAmount, DamagePopupType damageType)
    {
        if (damageType == DamagePopupType.Blocked)
        {            
            //Blocked Hit
            textMesh.SetText("Block");
            textMesh.fontSize = 20;
            textColor = new Color(72, 219, 255);
        }
        else if (damageType == DamagePopupType.Heal)
        {
            //Healing
            textMesh.SetText(popupAmount.ToString());
            textMesh.fontSize = 14;
            textColor = Color.green;
        }

        else if (damageType == DamagePopupType.Critical)
        {
            //Critical Hit
            textMesh.SetText(popupAmount.ToString() + "!");
            textMesh.fontSize = 20;
            textColor = new Color(255, 197, 0);
        }
        else if (damageType == DamagePopupType.Damage)
        {
            //Regular Damage
            textMesh.SetText(popupAmount.ToString());
            textMesh.fontSize = 14;
            textColor = Color.red;
        }

        textMesh.color = textColor;
        disappearTimer = DISSAPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(1f, 0.2f) * 3f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 1.5f * Time.deltaTime;

        if (disappearTimer > DISSAPEAR_TIMER_MAX * 0.5f)
        {
            //First half o the popup lifetime
            float increaseScaleAmount = 0.5f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            //Second half o the popup lifetime
            float decreaseScaleAmount = 0.5f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //Start disappearing
            float disappearSpeed = 4f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}