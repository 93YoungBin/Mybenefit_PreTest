using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesImageLoader : IResourceLoader<Sprite>
{
    private readonly Dictionary<string, Sprite> _cache = new Dictionary<string, Sprite>();

    public IEnumerator Co_Load(string path, Action<Sprite> onSuccess, Action<string> onFail)
    {
        if (_cache.TryGetValue(path, out Sprite cached))
        {
            onSuccess?.Invoke(cached);
            yield break;
        }

        ResourceRequest request = Resources.LoadAsync<Sprite>(path);

        yield return request;

        Sprite sprite = request.asset as Sprite;

        if (sprite == null)
        {
            onFail?.Invoke($"Can't Find Image : {path}");
            yield break;
        }

        _cache[path] = sprite;

        onSuccess?.Invoke(sprite);
    }
}
