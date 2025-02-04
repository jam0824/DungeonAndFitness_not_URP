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

        /// <summary>
        /// basePositionからtargetPositionを向く方向のベクトルを返す。
        /// </summary>
        /// <param name="basePosition"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        static public Vector3 GetDirection(Vector3 basePosition, Vector3 targetPosition) {
            return targetPosition - basePosition;
        }

        static public List<string[]> LoadCsvFile(string fileName) {
            List<string[]> tsvDatas = new List<string[]>();
            TextAsset tsvFile = Resources.Load(fileName) as TextAsset;
            StringReader reader = new StringReader(tsvFile.text);
            while (reader.Peek() != -1) {
                string line = reader.ReadLine();
                tsvDatas.Add(line.Split('\t'));
            }
            return tsvDatas;
        }

        //tsvのファイルを読み込む。string[]のList型で返す。
        static public List<string[]> LoadTsvFileFromTextAsset(TextAsset tsvFile) {
            List<string[]> tsvDatas = new List<string[]>();
            StringReader reader = new StringReader(tsvFile.text);
            while (reader.Peek() != -1) {
                string line = reader.ReadLine();
                string[] data = line.Split('\t');
                tsvDatas.Add(data);
            }
            return tsvDatas;
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

        //ファイルリード。ファイルがない場合は空ファイルを作成する
        static public List<string> LoadSaveFile(string fileName) {
            List<string> textDatas = new List<string>();
            string path = Application.persistentDataPath + "/" + fileName;
            //ファイルが存在しない場合、そのファイルを作って空のListを返す。
            if (System.IO.File.Exists(path) == false) {
                File.AppendAllText(path, "");
                return textDatas;
            }
            string data = File.ReadAllText(path);
            data = data.Replace("\r", "");
            string[] lines = data.Split("\n");
            textDatas.AddRange(lines);
            return DeleteBlankList(textDatas);
        }

        //空のリストを削除する
        static List<string> DeleteBlankList(List<string> list) {
            List<string> returnList = new List<string>();
            foreach (string tmp in list) {
                if (tmp == "") continue;
                returnList.Add(tmp);
            }
            return returnList;
        }

        //追記でセーブする
        static public void AppendStringFile(string fileName, string addString) {
            string path = Application.persistentDataPath + "/" + fileName;
            File.AppendAllText(path, addString + "\n");
        }

        //来たテキストをそのまま保存する
        static public void SaveStringToFile(string fileName, string text) {
            string path = Application.persistentDataPath + "/" + fileName;
            Debug.Log("save path:" + path);
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// SourcePositionからtargetPositionに向かってdistanceだけ近づいた位置を返す
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="sourcePosition"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        static public Vector3 GetPositionCloserToPoint(
            float distance,
            Vector3 sourcePosition,
            Vector3 targetPosition) 
        {
            Vector3 direction = (targetPosition - sourcePosition).normalized * distance;
            return sourcePosition + direction;
        }
    }
}