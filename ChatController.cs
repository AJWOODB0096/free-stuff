using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Threading.Tasks;

public class ChatController : MonoBehaviour
{
    // UI components
    public InputField chatInput;
    public Text chatOutput;
    public Button submitButton;

    // AI server URL
    private string aiServerUrl = "https://api.meta.ai/chat";

    // Event handler for when the user submits a message
    private void Start()
    {
        submitButton.onClick.AddListener(SubmitMessage);
    }

    // Submit the user's message to the AI server
    private async void SubmitMessage()
    {
        string message = chatInput.text.Trim();
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        // Send the message to the AI server
        await SendMessageToAIServer(message);

        // Clear the chat input field
        chatInput.text = "";
    }

    // Send the message to the AI server and get the response
    private async Task SendMessageToAIServer(string message)
    {
        // Create a new HTTP request
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(aiServerUrl, message))
        {
            // Send the request
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            // Check for errors
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the response from the AI server
                string response = request.downloadHandler.text;
                Debug.Log("Response from AI server: " + response);

                // Update the chat output text
                UpdateChatOutput("You: " + message + "\nAI: " + response + "\n");
            }
            else
            {
                Debug.LogError("Error sending message to AI server: " + request.error);
                UpdateChatOutput("Error: Failed to send message.\n");
            }
        }
    }

    // Update the chat output text on the main thread
    private void UpdateChatOutput(string message)
    {
        if (chatOutput != null)
        {
            chatOutput.text += message;
        }
    }
}
