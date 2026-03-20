using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inventory;

    public void OnInventoryOpen()
    {
        inventory.SetActive(!inventory.activeSelf);
    }
}
