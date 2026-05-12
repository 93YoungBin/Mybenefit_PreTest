using System;
using System.Collections;
using UnityEngine;

public interface IDataLoader
{
    void Load(Action<MachineData> onSuccess, Action<string> onFail);

    IEnumerator Co_Load(Action<MachineData> onSuccess, Action<string> onFail);
}
