using System;
using Events;
using UnityEngine;
using Wright.Library.Messages;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Wright.Library.File;

namespace Models
{
    public class DataExportModel : MonoBehaviour
    {
        [SerializeField] private string filename = "game";
        [SerializeField] private string extension = "dat";
        
        private void OnEnable()
        {
            Debug.Log("DM Enable");
            Messenger.AddListener(PresenterToModel.EXITING_GAME, SaveGameStatus);
        }

        private void OnDisable()
        {
            Debug.Log("DM Disable");
            Messenger.RemoveListener(PresenterToModel.EXITING_GAME, SaveGameStatus);
        }
        
        private void SaveGameStatus()
        {
            var datedFilename = FilenameSanitizer.Sanitize($"{filename}_{DateTime.Now}.{extension}");
            var fullFilename = Path.Combine(Application.persistentDataPath, datedFilename);
            Debug.Log($"Saving file to {fullFilename}");
            var gameState = new Dictionary<string, object> { { "time", LoadedModels.Measurement.ElapsedTimeSeconds } };

            using var stream = File.Create(fullFilename);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, gameState);
        }
    }
}