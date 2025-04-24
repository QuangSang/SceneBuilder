using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class ResourceLoader
{
    private List<AsyncOperationHandle> AssetHandles { get; } = new();
    private List<AsyncOperationHandle<GameObject>> GameObjectHandles { get; } = new();
    private List<AsyncOperationHandle<SceneInstance>> SceneHandles { get; } = new();

    public ResourceLoader()
    {
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
}