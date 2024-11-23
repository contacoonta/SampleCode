
using System;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
    using UnityEditor;
#endif


public class Serializer
{

#if UNITY_EDITOR
    public static void OpenPathWindow() => EditorUtility.RevealInFinder(Fullpath(null));
#endif

    // FILE PATH
    public static string Fullpath(string filenameNoExt)
    {
        string filepath = string.Empty;

        //EDITOR
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            filepath = Application.streamingAssetsPath + "/";
        }
        //ANDROID or IOS
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            filepath = Application.persistentDataPath + "/";
        }

        return filepath + filenameNoExt;
    }

    // FILE DELETE
    public static bool DeleteFile(string filenameNoExt, bool binary = true)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        try
        {
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                //Debug.Log($"File Deleted: {fullpath}");
                return true;
            }
            else
            {
                //Debug.LogWarning($"File not found: {fullpath}");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Serializer Delete FAILED ] {e} \n {fullpath}");
            return false;
        }
    }



    // Custom Class

    public static bool LoadJson<T>(string filenameNoExt, out T customObject, bool binary = true)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        try
        {
            if (!File.Exists(fullpath))
            {
                //Debug.LogWarning($"File not found: {fullpath}");
                customObject = default; // Return default value for type T (e.g., null for reference types)
                return false;
            }

            if (!binary)
            {
                string jstr = File.ReadAllText(fullpath);
                customObject = JsonUtility.FromJson<T>(jstr);
            }
            else
            {
                using (var stream = new FileStream(fullpath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    string jstr = (string)formatter.Deserialize(stream);
                    customObject = JsonUtility.FromJson<T>(jstr);
                }
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Load failed: {e} \n {fullpath}");
            customObject = default;
            return false;
        }
    }

    public static bool SaveJson<T>(string filenameNoExt, T customObject, bool binary = true)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        try
        {
            if (!binary)
            {
                string jstr = JsonUtility.ToJson(customObject, true);
                File.WriteAllText(fullpath, jstr);

                //Debug.Log($"Serializer Save Json SUCCESS ] {fullpath}");
            }
            else
            {
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    string jstr = JsonUtility.ToJson(customObject);

                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, jstr);
                }

                //Debug.Log($"Serializer Save Binary SUCCESS ] {fullpath}");
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Serializer Save FAILED ] {e} \n {fullpath}");

            return false;
        }
    }




    // Sync
    public static bool LoadJson(string filenameNoExt, ScriptableObject so, bool binary = true)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        if (!File.Exists(fullpath))
        {
            Debug.LogWarning($"Serializer Load Json FAILED ] Can't find File : {fullpath}");
            return false;
        }

        try
        {
            if (!binary)
            {
                string jstr = File.ReadAllText(fullpath);
                JsonUtility.FromJsonOverwrite(jstr, so);                

                //Debug.Log($"Serializer Load Json SUCCESS ] {fullpath}");
            }
            else
            {
                using (var stream = new FileStream(fullpath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    string jstr = (string)formatter.Deserialize(stream);
                    JsonUtility.FromJsonOverwrite(jstr, so);                    
                }

                //Debug.Log($"Serializer Load Binary SUCCESS ] {fullpath}");
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Serializer Load FAILED ] {e} \n {fullpath}");

            return false;
        }

    }

    public static bool SaveJson(string filenameNoExt, ScriptableObject so, bool binary = true)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        try
        {
            if (!binary)
            {
                string jstr = JsonUtility.ToJson(so, true);
                File.WriteAllText(fullpath, jstr);

                //Debug.Log($"Serializer Save Json SUCCESS ] {fullpath}");
            }
            else
            {
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    string jstr = JsonUtility.ToJson(so);

                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, jstr);
                }

                //Debug.Log($"Serializer Save Binary SUCCESS ] {fullpath}");
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Serializer Save FAILED ] {e} \n {fullpath}");

            return false;
        }
    }





    // Async 
    public static async UniTask<bool> LoadJsonAsync(string filenameNoExt, ScriptableObject so, bool binary = true, CancellationToken cancellationToken = default)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        if (!File.Exists(fullpath))
        {
            Debug.LogWarning($"Serializer LoadFromFile FAILED ] Can't find File : {fullpath}");
            return false;
        }

        try
        {
            await UniTask.SwitchToThreadPool();

            cancellationToken.ThrowIfCancellationRequested();

            if (!binary)
            {
                string jstr = await File.ReadAllTextAsync(fullpath, cancellationToken);
                JsonUtility.FromJsonOverwrite(jstr, so);

                //Debug.Log($"Serializer Load Json SUCCESS ] {fullpath}");
            }
            else
            {
                using (var stream = new FileStream(fullpath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    string jstr = (string)formatter.Deserialize(stream);
                    JsonUtility.FromJsonOverwrite(jstr, so);
                }

                //Debug.Log($"Serializer Load Binary SUCCESS ] {fullpath}");
            }

            await UniTask.SwitchToMainThread();

            return true;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"Serializer LoadFromFile CANCELED ] {fullpath}");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Serializer LoadFromFile ERROR ] {fullpath} : {e}");
            return false;
        }
        finally
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }


    // SAVE FILE
    public static async UniTask<bool> SaveJsonAsync(string filenameNoExt, ScriptableObject so, bool binary = true, CancellationToken cancellationToken = default)
    {
        string fullpath = Fullpath(filenameNoExt) + (binary ? ".bin" : ".json");

        try
        {
            await UniTask.SwitchToThreadPool();

            cancellationToken.ThrowIfCancellationRequested();

            if (!binary)
            {
                string jstr = JsonUtility.ToJson(so, true);
                await File.WriteAllTextAsync(fullpath, jstr, cancellationToken);

                //Debug.Log($"Serializer Save Json SUCCESS ] {fullpath}");
            }
            else
            {
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    string jstr = JsonUtility.ToJson(so);

                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, jstr);
                }

                //Debug.Log($"Serializer Save Binary SUCCESS ] {fullpath}");
            }

            await UniTask.SwitchToMainThread();

            return true;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"Serializer SaveToFile CANCELED ] {fullpath}");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Serializer SaveToFile ERROR ] {fullpath} : {e}");
            return false;
        }
        finally
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

}