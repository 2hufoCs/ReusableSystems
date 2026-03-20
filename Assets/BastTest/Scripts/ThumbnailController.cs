using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailController : MonoBehaviour
{
    [SerializeField] ThumbnailData firstThumbnail;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform choicePanelTransform;

    [SerializeField] Image thumbnail;
    [SerializeField] TextMeshProUGUI description;

    void Start()
    {
        DisplayThumbnail(firstThumbnail);
    }

    /// <summary>
    /// Displays thumbnail data, and loads all choices and sub-choices
    /// </summary>
    /// <param name="data"></param>
    void DisplayThumbnail(ThumbnailData data)
    {
        thumbnail.sprite = data.thumbnailImage;
        description.text = data.description;

        // Destroy pre-existing buttons
        foreach (Transform child in choicePanelTransform)
        {
            Destroy(child.gameObject);
        }

        // Show other buttons (recursively)
        foreach (ChoiceData choiceData in data.choicesData)
        {
            GameObject instantiate = Instantiate(buttonPrefab, choicePanelTransform);

            // Check if criterias are met (haas necessary items and no trap items)

            // Add item
            if (choiceData.behaviours.HasFlag(ChoiceBehaviours.AddItem))
            {
                Debug.Log(choiceData.behaviours);
                Inventory.Instance.SpawnItem(choiceData.itemToAdd);
            }
        
            instantiate.GetComponentInChildren<TextMeshProUGUI>().text = choiceData.choice;
            instantiate.GetComponent<Button>().onClick.AddListener( () => { DisplayThumbnail(choiceData.linkedThumbnail); });
        }
    }
}