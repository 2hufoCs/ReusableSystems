using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class ThumbnailController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ThumbnailData firstThumbnail;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform choicePanelTransform;
    [SerializeField] TextMeshProUGUI reputationText;

    [Header("Currently Displayed")]
    [SerializeField] Image thumbnail;
    [SerializeField] TextMeshProUGUI description;

    private int reputation;

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
            /*
            Debug.Log("checking conditions of new choice");
            Debug.Log(!choiceData.behaviours.HasFlag(ChoiceBehaviours.MustHaveItem) + ", " + Inventory.Instance.FindHeldItem(choiceData.itemToHave));
            Debug.Log(!choiceData.behaviours.HasFlag(ChoiceBehaviours.MustNotHaveItem) + ", " + !Inventory.Instance.FindHeldItem(choiceData.itemToNotHave));
            Debug.Log(!(choiceData.triggerOnce && choiceData.triggered));
            */
            
            // Enable/Disable button depending on item requirements
            bool btnEnabled = !choiceData.behaviours.HasFlag(ChoiceBehaviours.MustHaveItem) || Inventory.Instance.FindHeldItems(choiceData.itemsToHave);
            btnEnabled &= !choiceData.behaviours.HasFlag(ChoiceBehaviours.MustNotHaveItem) || !Inventory.Instance.FindHeldItems(choiceData.itemsToNotHave);
            btnEnabled &= !(choiceData.triggerOnce && choiceData.triggered);
            if (!btnEnabled && choiceData.hideOnDisabled) continue; // Hide button

            GameObject newButton = Instantiate(buttonPrefab, choicePanelTransform);
            Button btn = newButton.GetComponent<Button>();
            btn.interactable = btnEnabled;
            
            // Show text, and add listener when button is clicked
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = choiceData.choice;
            btn.onClick.AddListener( () => { ExecuteChoice(choiceData); });
        }

        void ExecuteChoice(ChoiceData choice)
        {
            // Trigger for one-time only choices
            if (choice.triggerOnce) choice.triggered = true;

            // Roll choice with probability
            if (choice.behaviours.HasFlag(ChoiceBehaviours.ProbabilityEvent))
            {
                ChoiceData newChoice = RollEvent(choice);
                if (choice != newChoice)
                {
                    ExecuteChoice(newChoice);
                    return;
                }
            }

            // Get different choices automatically depending on items
            if (choice.behaviours.HasFlag(ChoiceBehaviours.ChoiceInventoryChange))
            {
                foreach (ItemsNames itemName in choice.alternateChoices.Keys)
                {
                    if (!Inventory.Instance.FindHeldItem(itemName)) continue;
                    ExecuteChoice(choice.alternateChoices[itemName]);
                    return;
                }
            }

            // Reputation Change
            if (choice.behaviours.HasFlag(ChoiceBehaviours.ReputationChange))
            {
                reputation += choice.reputationPoints;
                reputationText.text = "Reputation: " + reputation;
            }

            // Change scene depending on reputation
            if (choice.behaviours.HasFlag(ChoiceBehaviours.ReputationRequirement)) choice = (reputation >= choice.reputationThreshold) ? choice : choice.badReputationScene;

            // Add item
            if (choice.behaviours.HasFlag(ChoiceBehaviours.AddItem)) Inventory.Instance.SpawnItem(choice.itemToAdd);

            // Remove item
            if (choice.behaviours.HasFlag(ChoiceBehaviours.RemoveItem)) Inventory.Instance.RemoveItem(choice.itemToRemove);

            // Perform certain actions depending on the choice behaviours
            // switch (choice.behaviours.HasFlag)
            // {
            //     // Roll choice with probability
            //     case ChoiceBehaviours.ProbabilityEvent:
            //         choice = RollEvent(choice);
            //         break;

            //     // Add item
            //     case ChoiceBehaviours.AddItem:
            //         Inventory.Instance.SpawnItem(choice.itemToAdd);
            //         break;

            //     // Remove item
            //     case ChoiceBehaviours.RemoveItem:
            //         Inventory.Instance.RemoveItem(choice.itemToRemove);
            //         break;

            //     // Change reputation
            //     case ChoiceBehaviours.ReputationChange:
            //         reputation += choice.reputationPoints;
            //         reputationText.text = "Reputation: " + reputation;
            //         break;
            // }
            

            // Once all items-related tasks have been executed, display next thumbnail
            DisplayThumbnail(choice.linkedThumbnail);
        }

        // Roll event with given probability
        ChoiceData RollEvent(ChoiceData choiceToRoll)
        {
            int rnd = UnityEngine.Random.Range(1, 101);
            Debug.Log("rolled a " + rnd);
            ChoiceData finalChoice = rnd < choiceToRoll.winRate ? choiceToRoll : choiceToRoll.loseScene;
            return finalChoice;
        }
    }
}