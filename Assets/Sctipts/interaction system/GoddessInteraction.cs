using UnityEngine;
using TMPro;

public class GoddessInteraction : MonoBehaviour
{
    [Header("공용 UI 텍스트")]
    public TextMeshProUGUI interactionText;

    [Header("머리 위 위치")]
    public Transform headPoint;
    public Vector3 textOffset = new Vector3(0f, 2.0f, 0f);

    [Header("입력 키")]
    public KeyCode interactKey = KeyCode.Y;

    [Header("대사")]
    [TextArea] public string idleLine = "기도드리시겠습니까? [Y]";
    [TextArea] public string completeLine = "기도 완료!";

    private bool playerInRange = false;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            UpdateTextPosition();

            if (Input.GetKeyDown(interactKey))
            {
                Interact();
            }
        }
    }

    private void Interact()
    {
        ShowText(completeLine);
        Debug.Log("기도 완료!");
    }

    private void ShowText(string message)
    {
        if (interactionText == null)
            return;

        interactionText.gameObject.SetActive(true);
        interactionText.text = message;
        UpdateTextPosition();
    }

    private void HideText()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void UpdateTextPosition()
    {
        if (interactionText == null || !interactionText.gameObject.activeSelf)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 worldPos = headPoint != null ? headPoint.position : transform.position + textOffset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        if (screenPos.z > 0f)
        {
            interactionText.transform.position = screenPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        ShowText(idleLine);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        HideText();
    }

    private void OnDisable()
    {
        HideText();
    }
}