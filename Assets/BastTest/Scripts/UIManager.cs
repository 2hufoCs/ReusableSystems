using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inventory;

    void Awake()
    {
        inventory.SetActive(true);
        inventory.SetActive(false);
    }

    public void OnInventoryOpen()
    {
        inventory.SetActive(!inventory.activeSelf);
    }
}
