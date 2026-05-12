using System;
using System.Collections;
using UnityEngine;

public class DataLoader : IDataLoader
{
    private const string ResourcePath = "Items";

    //코루틴을 사용한 비동기 로드
    public IEnumerator Co_Load(Action<MachineData> onSuccess, Action<string> onFail)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(ResourcePath);
        yield return request;

        TextAsset json = request.asset as TextAsset;

        if (json == null)
        {
            onFail?.Invoke("파일을 찾을 수 없습니다.");
            yield break;
        }

        MachineData data = JsonUtility.FromJson<MachineData>(json.text);

        if (data == null)
        {
            onFail?.Invoke("JSON 파싱 실패.");
            yield break;
        }

        onSuccess?.Invoke(data);
    }

    //일반 로드
    public void Load(Action<MachineData> onSuccess, Action<string> onFail)
    {
        TextAsset json = Resources.Load<TextAsset>(ResourcePath);

        if (json == null)
        {
            onFail?.Invoke("파일을 찾을 수 없습니다.");
            return;
        }

        MachineData data = JsonUtility.FromJson<MachineData>(json.text);

        if (data == null)
        {
            onFail?.Invoke("JSON 파싱 실패.");
            return;
        }

        onSuccess?.Invoke(data);
    }
}
