using UnityEngine;
using System.Diagnostics;

public class EchoVoice : MonoBehaviour
{
    private Process ttsProcess;

    public void Speak(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        // Clean the text for PowerShell
        string cleanText = text.Replace("'", "\\'").Replace("\"", "\\\"");

        try
        {
            // Use PowerShell + Windows built-in TTS
            string psCommand = $"-Command \"Add-Type -AssemblyName System.Speech; " +
                               "$speak = New-Object System.Speech.Synthesis.SpeechSynthesizer; " +
                               "$speak.Speak('{cleanText}');\"";

            ttsProcess = new Process();
            ttsProcess.StartInfo.FileName = "powershell.exe";
            ttsProcess.StartInfo.Arguments = psCommand;
            ttsProcess.StartInfo.CreateNoWindow = true;
            ttsProcess.StartInfo.UseShellExecute = false;
            ttsProcess.StartInfo.RedirectStandardOutput = true;
            ttsProcess.Start();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogWarning("TTS failed: " + e.Message);   // Fixed: Use UnityEngine.Debug
        }
    }

    private void OnDestroy()
    {
        try
        {
            if (ttsProcess != null && !ttsProcess.HasExited)
                ttsProcess.Kill();
        }
        catch { }
    }
}
