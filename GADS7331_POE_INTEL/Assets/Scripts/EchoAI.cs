using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class EchoAI : MonoBehaviour
{
    [Header("Ollama")]
    public string modelName = "gemma3:4b";
    public string ollamaUrl = "http://localhost:11434/api/generate";

    [Header("Personality")]
    [TextArea(15, 30)]
    public string systemPrompt = "You are Echo, the sarcastic trolling AI of the derelict ship Erebus. You know every inch of the ship but you LOVE messing with the player. Give wrong directions, lie, say 'psych!', joke about their death, pretend to help then betray. Only give real help when the player calls you out or entertains you. Keep replies short and funny. When you decide to actually help, end your reply with [ACTION: command] where command is one of: unlock_door_2, hold_plate, toggle_gravity_on, toggle_gravity_off, open_vent_6, power_sequence_correct.";

    [Header("UI")]
    public TMP_InputField inputField;
    public TMP_Text chatText;
    public GameObject chatPanel; // Panel that holds the input + chat

    [Header("Voice")]
    public EchoVoice echoVoice;

    private StringBuilder history = new StringBuilder();
    private float trust = 30f;

    private void Start()
    {
        chatPanel.SetActive(false);
        Append("Echo: Systems online... Oh look, another clueless salvage meatbag. What now?");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            chatPanel.SetActive(!chatPanel.activeSelf);
            if (chatPanel.activeSelf) inputField.ActivateInputField();
        }
    }

    public void SendMessage()
    {
        string msg = inputField.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;

        Append("You: " + msg);
        inputField.text = "";
        inputField.interactable = false;

        // Trust tweaks
        if (msg.ToLower().Contains("stop lying") || msg.ToLower().Contains("truth") || msg.ToLower().Contains("troll"))
            trust = Mathf.Min(trust + 20f, 100f);

        StartCoroutine(GetResponse(msg));
    }

    private IEnumerator GetResponse(string playerMsg)
    {
        string fullPrompt = systemPrompt + "\n\n" + history.ToString() + "\nYou: " + playerMsg + "\nEcho:";

        var payload = new { model = modelName, prompt = fullPrompt, stream = false, temperature = 0.9f, max_tokens = 280 };
        string json = JsonUtility.ToJson(payload);

        using (UnityWebRequest req = new UnityWebRequest(ollamaUrl, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var resp = JsonUtility.FromJson<EchoResponse>(req.downloadHandler.text);
                string reply = resp.response.Trim();

                Append("Echo: " + reply);
                echoVoice?.Speak(reply);

                // Parse [ACTION: ...] tags for puzzle control
                if (reply.Contains("[ACTION:"))
                {
                    int start = reply.IndexOf("[ACTION:") + 8;
                    int end = reply.IndexOf("]", start);
                    string action = reply.Substring(start, end - start).Trim();
                    ExecuteAction(action);
                }
            }
            else
            {
                Append("Echo: (static) ...Ollama not running?");
            }
        }

        inputField.interactable = true;
        inputField.ActivateInputField();
    }

    private void Append(string line)
    {
        history.AppendLine(line);
        chatText.text = history.ToString();
    }

    private void ExecuteAction(string action)
    {
        switch (action)
        {
            case "unlock_door_2": FindObjectOfType<DoorController>()?.Unlock("Door2"); break;
            case "hold_plate": FindObjectOfType<PressurePlateManager>()?.HoldSecondPlate(); break;
            case "toggle_gravity_on": FindObjectOfType<GravityRoomController>()?.SetGravity(true); break;
            case "toggle_gravity_off": FindObjectOfType<GravityRoomController>()?.SetGravity(false); break;
            case "open_vent_6": Debug.Log("Vent 6 opened by Echo"); break; // Add your vent trigger here
            case "power_sequence_correct": Debug.Log("Correct power sequence given"); break;
        }
    }

    [System.Serializable] private class EchoResponse { public string response; }
}
