using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    private bool _inventoryOpen;

    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        _inventoryOpen = !_inventoryOpen;
        inventoryPanel.SetActive(_inventoryOpen);
    }
}
