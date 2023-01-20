using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FQCommon
{
    public class Common : MonoBehaviour
    {
        static public int GetDamage(int atk, int def) {
            float damage = Mathf.Floor(atk / 2 - def / 4);
            return (int)damage;
        }

        static public float GetDistance(Vector3 startPos, Vector3 endPos) {
            float xDist = Mathf.Pow(startPos.x - endPos.x, 2);
            float yDist = Mathf.Pow(startPos.y - endPos.y, 2);
            float zDist = Mathf.Pow(startPos.z - endPos.z, 2);
            return Mathf.Sqrt(xDist + zDist + yDist);
        }

        static public List<string[]> LoadCsvFile(string fileName) {
            List<string[]> csvDatas = new List<string[]>();
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;
            StringReader reader = new StringReader(csvFile.text);
            while (reader.Peek() != -1) {
                string line = reader.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            return csvDatas;
        }
        static public List<string[]> LoadCsvFileFromTextAsset(TextAsset csvFile) {
            List<string[]> csvDatas = new List<string[]>();
            StringReader reader = new StringReader(csvFile.text);
            while (reader.Peek() != -1) {
                string line = reader.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            return csvDatas;
        }

        static public List<string> LoadTextFile(string fileName) {
            List<string> textDatas = new List<string>();
            TextAsset textFile = Resources.Load(fileName) as TextAsset;
            StringReader reader = new StringReader(textFile.text);
            while (reader.Peek() != -1) {
                string line = reader.ReadLine();
                line = line.Replace("\r", "");
                line = line.Replace("\n", "");
                textDatas.Add(line);
            }
            return textDatas;
        }
    }
}