using System.IO;
using PCLoveGame.Core;
using PCLoveGame.Data;
using UnityEngine;

namespace PCLoveGame.SaveSystem
{
    public sealed class SaveService
    {
        private readonly string _saveFilePath;

        public SaveService()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, GameConstants.SaveFileName);
        }

        public string SaveFilePath => _saveFilePath;

        public GameSaveData Load()
        {
            if (!File.Exists(_saveFilePath))
            {
                return new GameSaveData();
            }

            try
            {
                string json = File.ReadAllText(_saveFilePath);
                GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
                return saveData ?? new GameSaveData();
            }
            catch (IOException exception)
            {
                Debug.LogWarning($"[SaveService] 读取存档失败：{exception.Message}");
                return new GameSaveData();
            }
        }

        public void Save(GameSaveData saveData)
        {
            if (saveData == null)
            {
                Debug.LogWarning("[SaveService] 保存失败：存档数据为空。");
                return;
            }

            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(_saveFilePath, json);
            }
            catch (IOException exception)
            {
                Debug.LogWarning($"[SaveService] 写入存档失败：{exception.Message}");
            }
        }
    }
}
