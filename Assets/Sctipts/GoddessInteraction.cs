using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoddessInteraction : MonoBehaviour
{
    public TextMeshProUGUI interactionText;

    private bool playerInRange = false;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // 플레이어가 범위 안에 있을 때만 입력 받기
        if (playerInRange && Input.GetKeyDown(KeyCode.Y))
        {
            Interact();
        }
    }

    private void Interact()
    {
        // 상호작용 결과
        interactionText.text = "기도 완료!";

        Debug.Log("기도 완료!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "기도드리시겠습니까? [Y]";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}