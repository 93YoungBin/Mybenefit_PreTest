using System;
using System.Collections;

public interface IResourceLoader<T>
{
    //추후 코루틴을 제외한 동기 상황에서 사용 할 경우 해제
    //void Load(string path, Action<T> onSuccess, Action<string> onFail);
    IEnumerator Co_Load(string path, Action<T> onSuccess, Action<string> onFail);
}
