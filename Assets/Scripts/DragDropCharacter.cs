using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,IEndDragHandler, IDragHandler, IPointerEnterHandler ,IPointerExitHandler
{
    public Canvas canvas;
    private RectTransform rectTransform;
    [SerializeField] private Image highlightShadow;
    private Color dragColor;
    private Color highlightColor;

    private CanvasGroup canvasGroup;

    private Vector3 savedCoordinatesBeginDraging;

    private bool dragging;

    private void Awake()
    {
        highlightColor = new Color(0,196,255,168);
        dragColor = new Color(255, 236, 0, 168);
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        dragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(highlightColor != null)
        {
            dragging = true;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3) eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (highlightColor != null)
        {
            canvasGroup.blocksRaycasts = true;
            gameObject.transform.position = savedCoordinatesBeginDraging;
            highlightShadow.gameObject.SetActive(false);
            highlightShadow.color = highlightColor;
            dragging = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        savedCoordinatesBeginDraging = gameObject.transform.position;
        Debug.LogError(savedCoordinatesBeginDraging);
        rectTransform.position = Input.mousePosition;
        highlightShadow.color = dragColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightColor != null)
        {
            highlightShadow.gameObject.SetActive(true);
            highlightShadow.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightColor != null)
        {
            if (!dragging)
                highlightShadow.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
