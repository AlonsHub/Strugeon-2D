using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInitializer : MonoBehaviour
{
    [SerializeField, Tooltip("There SHOULD NOT be an NPC conversation component on the GameObject called Conversation - you should add it whenever you add this prefab to a scene, add a new NPCConversation component and drag it here!")]
    NPCConversation npcConversation;
    [SerializeField]
    ConversationManager conversationManager;
    [SerializeField]
    Image bgImage;
    [SerializeField]
    bool startConversationOnAwake;

    [SerializeField]
    Transform rightMark;
    [SerializeField]
    Transform leftMark;

    [SerializeField, Tooltip("Leave null to disable BG_Image on awake")]
    Sprite bg_sprite;


    void Awake()
    {
        if (bgImage)
        {
            if (!bg_sprite)
                bgImage.gameObject.SetActive(false);
            else
                bgImage.sprite = bg_sprite;
        }

        if (!conversationManager)
            conversationManager = GetComponentInChildren<ConversationManager>();
        if (!npcConversation)
            npcConversation = GetComponentInChildren<NPCConversation>();

        if (!conversationManager || !npcConversation)
        {
            Debug.LogError("Missing components!");
            return;
        }

        Invoke(nameof(LateStartConversation), 1f);
    }

    private void LateStartConversation()
    {
        if (startConversationOnAwake)
        {
            conversationManager.StartConversation(npcConversation);
        }
    }
}
