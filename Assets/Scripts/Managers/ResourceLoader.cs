using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;


public class ResourceLoader
{
    private List<AsyncOperationHandle> AssetHandles { get; } = new();
    private List<AsyncOperationHandle<GameObject>> GameObjectHandles { get; } = new();
    private List<AsyncOperationHandle<SceneInstance>> SceneHandles { get; } = new();

    public ResourceLoader()
    {
    }

    public async Task<T> InstantiateAsyncGameObject<T>(string key, Transform parent = null)
    {
        return await ProcessInstantiateAsyncGameObject<T>(Addressables.InstantiateAsync(key, parent), key);
    }

    public async Task<T> InstantiateAsyncGameObject<T>(
        AssetReferenceGameObject goReference,
        Transform parent = null
    )
    {
        return await ProcessInstantiateAsyncGameObject<T>(
            Addressables.InstantiateAsync(goReference, parent),
            goReference.RuntimeKey.ToString()
        );
    }

    public async Task<T> InstantiateAsyncGameObject<T>(
        AssetReferenceGameObject goReference,
        Vector3 position,
        Quaternion rotation,
        Transform parent = null
    )
    {
        return await ProcessInstantiateAsyncGameObject<T>(
            Addressables.InstantiateAsync(goReference, position, rotation, parent),
            goReference.RuntimeKey.ToString()
        );
    }

    public bool IsEmpty()
    {
        return AssetHandles.Count == 0 && GameObjectHandles.Count == 0 && SceneHandles.Count == 0;
    }

    public async Task<T> LoadAssetAsync<T>(string key)
    {
        return await ProcessLoadAssetAsync(Addressables.LoadAssetAsync<T>(key), key);
    }

    public async Task<T> LoadAssetAsync<T>(AssetReference assetReference)
    {
        return await ProcessLoadAssetAsync(
            Addressables.LoadAssetAsync<T>(assetReference),
            assetReference.RuntimeKey.ToString()
        );
    }

    public async Task ReleaseAll(bool applicationExiting = false)
    {
        // Release assets.
        foreach (var assetHandle in AssetHandles)
        {
            if (CheckHandleOnRelease(assetHandle))
            {
                Addressables.Release(assetHandle);
            }
        }

        AssetHandles.Clear();

        // Release GO instances.
        foreach (var goHandle in GameObjectHandles)
        {
            if (CheckHandleOnRelease(goHandle))
            {
                Addressables.ReleaseInstance(goHandle);
            }
        }

        GameObjectHandles.Clear();
    }

    public void ReleaseAsset(Object asset)
    {
        AsyncOperationHandle assetHandle = default;

        foreach (var handle in AssetHandles)
        {
            if ((Object)handle.Result == asset)
            {
                assetHandle = handle;

                break;
            }
        }

        ReleaseAssetHandle(assetHandle);
    }

    public void ReleaseAssetHandle(AsyncOperationHandle assetHandle)
    {
        AssetHandles.Remove(assetHandle);
        Addressables.Release(assetHandle);
    }

    public void ReleaseGO(GameObject instance)
    {
        AsyncOperationHandle<GameObject> goHandle = default;

        foreach (var handle in GameObjectHandles)
        {
            if (handle.Result == instance)
            {
                goHandle = handle;

                break;
            }
        }

        if (goHandle.Equals(default))
        {
            return;
        }

        ReleaseGOHandle(goHandle);
    }

    public void ReleaseGOHandle(AsyncOperationHandle<GameObject> goHandle)
    {
        GameObjectHandles.Remove(goHandle);
        Addressables.ReleaseInstance(goHandle);
    }

    public bool TryRemoveHandleForGO(GameObject instance, out AsyncOperationHandle<GameObject> removedHandle)
    {
        removedHandle = new AsyncOperationHandle<GameObject>();

        for (var index = GameObjectHandles.Count - 1; index >= 0; index--)
        {
            if (GameObjectHandles[index].Result != instance)
            {
                continue;
            }

            removedHandle = GameObjectHandles[index];

            GameObjectHandles.RemoveAt(index);

            return true;
        }

        return false;
    }

    // Returns whether the handle is valid.
    private bool CheckHandleOnRelease(AsyncOperationHandle handle)
    {
        if (!handle.IsValid())
        {
            return false;
        }

        return true;
    }

    private async Task<T> ProcessInstantiateAsyncGameObject<T>(AsyncOperationHandle<GameObject> handle, string key)
    {
        GameObjectHandles.Add(handle);

        var result = await handle.Task;

        return result.GetComponent<T>();
    }

    private async Task<T> ProcessLoadAssetAsync<T>(AsyncOperationHandle<T> handle, string key)
    {
        AssetHandles.Add(handle);

        var result = await handle.Task;

        return result;
    }
}