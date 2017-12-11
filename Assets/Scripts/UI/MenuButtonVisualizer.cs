using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MenuButtonVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] Text buttonText;
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightColor;

    // Use this for initialization
    void Start () {
        buttonText.color = normalColor;
	}


    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
