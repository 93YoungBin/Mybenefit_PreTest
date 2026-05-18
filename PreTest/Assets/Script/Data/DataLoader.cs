using System;
using System.Collections;
using UnityEngine;

public class DataLoader : IResourceLoader<MachineData>
{
    public IEnumerator Co_Load(string path, Action<MachineData> onSuccess, Action<string> onFail)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(path);

        yield return request;

        TextAsset json = request.asset as TextAsset;

        if (json == null)
        {
            onFail?.Invoke($"File Not Found: {path}");
            yield break;
        }

        MachineData data = JsonUtility.FromJson<MachineData>(json.text);

        if (data == null)
        {
            onFail?.Invoke($"Json Parse Fail: {path}");
            yield break;
        }

        onSuccess?.Invoke(data);
    }
}
