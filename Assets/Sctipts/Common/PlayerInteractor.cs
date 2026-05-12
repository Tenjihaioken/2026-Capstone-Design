using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("상호작용 키")]
    public KeyCode interactKey = KeyCode.F;

    private IInteractable currentInteractable;
    private PlayerCore playerCore;

    private void Awake()
    {
        playerCore = GetComponent<PlayerCore>();
    }

    private void Update()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact(playerCore);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log(interactable.GetInteractText());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}