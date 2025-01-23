using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SignalSystem
{
    public class SignalDialogue : MonoBehaviour
    {
        [Header("Signals information")]
        public List<SignalObject> signalPool = new List<SignalObject>();

        void Start()
        {
            //LoadEventsFromFile("Assets/Data/signals.json");
            string path = Path.Combine(Application.streamingAssetsPath, "signals.json");
            LoadEventsFromFile(path);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LoadEventsFromFile(string path)
        {
            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                signalPool = JsonUtility.FromJson<SignalObjectArray>(jsonContent).signals;
            }
        }
    }
  
    [System.Serializable]
    public class SignalObjectArray
    {
        public List<SignalObject> signals;
    }

    [System.Serializable]
    public class SignalObject
    {
        public string type;
        public DialogueScript dialogue;
    }

    [System.Serializable]
    public class DialogueScript
    {
        public SignalLines[] spanish;
        public SignalLines[] english;
    }

    [System.Serializable]
    public class SignalLines
    {
        public string character;
        public string[] text;
    }
}

