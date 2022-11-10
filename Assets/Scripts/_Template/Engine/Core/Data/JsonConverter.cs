using System;
using UnityEngine;

namespace engine.utility
{
    public static class JsonConverter
    {
        public static bool JsonToObject<TObject>(this TObject obectConvert, string contents)
        {
            try
            {
                JsonUtility.FromJsonOverwrite(contents, obectConvert);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error: " + e.Message);
            }

            return false;
        }

        public static string ObjectToJson<TObject>(this TObject obectConvert)
        {
            try
            {
                return JsonUtility.ToJson(obectConvert);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error: " + e.Message);
            }

            return "{ }";
        }
    }
}