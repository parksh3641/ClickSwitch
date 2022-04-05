using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.IO;

public static class FileIO
{
    // 키로 사용하기 위한 암호 정의
    private static readonly string PASSWORD = "3os1s334e3kcc7c4yf4554e632983h";
    // 인증키 정의
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    /// <summary>
    /// 데이터 저장, 암호화 선택(암호화 자동)
    /// </summary>
    /// <param name="fileName">파일명</param>
    /// <param name="data">데이터</param>
    /// <param name="withEncryption">암호화 여부</param>
    public static void SaveData(string fileName, string data, bool withEncryption = false)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new System.ArgumentException("Parameter cannot be null", "fileName");
        }
        if (string.IsNullOrEmpty(data))
        {
            throw new System.ArgumentException("Parameter cannot be null", "data");
        }

        if (withEncryption)
        {
            SaveData(fileName, Encrypt(data));
        }
        else
        {
            SaveData(fileName, data);
        }
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    /// <param name="fileName">파일명</param>
    /// <param name="data">데이터</param>
    private static void SaveData(string fileName, string data)
    {
        string path = pathForDocumensFile(fileName);
        FileStream file = File.Open(path, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(data);
        sw.Close();
        file.Close();
    }

    /// <summary>
    /// 데이터 로드, 복호화 선택
    /// </summary>
    /// <param name="fileName">파일명</param>
    /// <param name="withDecryption">복호화 여부</param>
    /// <returns></returns>
    public static string LoadData(string fileName, bool withDecryption = false)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new System.ArgumentException("Parameter cannot be null", "fileName");
        }
        if (withDecryption)
        {
            string load = LoadData(fileName);
            if (!string.IsNullOrEmpty(load))
            {
                load = Decrypt(load);
            }
            return load;
        }
        else
        {
            return LoadData(fileName);
        }
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    /// <param name="fileName">파일명</param>
    /// <returns></returns>
    private static string LoadData(string fileName)
    {
        string data = string.Empty;
        string path = pathForDocumensFile(fileName);
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            data = sr.ReadLine();

            sr.Close();
            file.Close();
        }
        return data;
    }

    public static string LoadDataWithPath(string filePath)
    {
        string data = string.Empty;
        if (File.Exists(filePath))
        {
            FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            data = sr.ReadLine();

            sr.Close();
            file.Close();
        }
        return data;
    }

    /// <summary>
    /// 데이터 파일 제거 함수
    /// </summary>
    /// <param name="fileName">제거할 데이터 함수 이름</param>
    /// <returns>0은 데이터 없음. 1은 제거 성공</returns>
    public static bool DeleteData(string fileName)
    {
        string path = pathForDocumensFile(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 파일 위치 찾는 함수 외부 호출되지 않는다.
    /// </summary>
    /// <param name="FileName">찾는파일</param>
    /// <returns>해당 파일 경로</returns>
    private static string pathForDocumensFile(string FileName)
    {
        string path = null;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, FileName);
            case RuntimePlatform.IPhonePlayer:
                path = Application.persistentDataPath;
                //path = Application.persistentDataPath.Substring(0, Application.dataPath.Length - 5); ;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Path.Combine(path, "Documents"), FileName);
            default:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, FileName);
        }
    }

    /// <summary>
    /// 암호화 자동
    /// </summary>
    /// <param name="data">입력받은 데이터</param>
    /// <returns>암호화된 데이터</returns>
    private static string Encrypt(string data)
    {
        byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(data);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(System.Text.Encoding.UTF8.GetBytes(KEY), System.Text.Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();

        string encryptString = System.Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }



    /// <summary>
    /// 복호화 자동
    /// </summary>
    /// <param name="data">입력받은 암호화된 데이터</param>
    /// <returns>복호화된 데이터</returns>
    private static string Decrypt(string data)
    {
        byte[] encryptBytes = System.Convert.FromBase64String(data);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);

        ICryptoTransform decryptor = myRijndael.CreateDecryptor(System.Text.Encoding.UTF8.GetBytes(KEY), System.Text.Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = System.Text.Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }


}