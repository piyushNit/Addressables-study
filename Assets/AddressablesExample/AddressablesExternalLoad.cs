using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public enum Platform
{
    Windows = 0,
    Android
}

public enum AssetVersion
{
    Ver_1=0,
    Ver_2
}

public class AddressablesExternalLoad : MonoBehaviour
{
    public Platform platform;
    public AssetVersion assetVersion;

    public List<IResourceLocation> locations;

    public string assetKey = "OldPlanePrefab";

    void Start()
    {
        initAddressables();
    }

    void initAddressables()
    {
        Debug.Log("initAddressables");
        AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();
        handle.Completed += initDone;
    }
    private void initDone(AsyncOperationHandle<IResourceLocator> obj)
    {
        Debug.Log("Initialization Complete ==> " + obj.Status);
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            loadCatalog();
        }
    }

    string GetCatalogPath()
    {
        string catalogPath = "https://storage.googleapis.com/petpilot_addressables/";
        catalogPath += assetVersion == AssetVersion.Ver_1 ? "petpilot_v1/ServerData/" : "petpilot_v2/ServerData/";

        catalogPath += platform == Platform.Windows ? "StandaloneWindows64/" : "Android/";
        catalogPath += assetVersion == AssetVersion.Ver_1 ? "catalog_1.0.json" : "catalog_2.0.json";

        return catalogPath;
    }

    void loadCatalog()
    {
        string catalogPath = GetCatalogPath();
        Debug.Log("loadCatalog: " + catalogPath);
        AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(catalogPath);
        handle.Completed += loadCatalogsCompleted;
    }
    void loadCatalogsCompleted(AsyncOperationHandle<IResourceLocator> obj)
    {
        Debug.Log("loadCatalogsCompleted ==> " + obj.Status);
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("loadResourceLocation");
            var keys = obj.Result.Keys;
            Debug.Log("Key: " + keys.ToString());
            AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(assetKey);
            handle.Completed += locationsLoaded;
        }
        else
        {
            Debug.LogError("LoadCatalogsCompleted is failed");
        }
    }

    void locationsLoaded(AsyncOperationHandle<IList<IResourceLocation>> obj)
    {
        Debug.Log("locationsLoaded ==> " + obj.Status);
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            locations = new List<IResourceLocation>(obj.Result);
            loadDependency();
        }
        else
        {
            Debug.LogError("locationsLoaded is failed");
        }
    }

    void loadDependency()
    {
        Debug.Log("loadDependency");
        AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(assetKey, true);
        handle.Completed += dependencyLoaded;
    }
    void dependencyLoaded(AsyncOperationHandle obj)
    {
        Debug.Log("dependencyLoaded ==> " + obj.Status);
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            loadAssets();
        }
        else
        {
            Debug.LogError("dependencyLoaded is Failed");
        }
    }

    private void loadAssets()
    {
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(locations, onAssetsCategoryLoaded);
        handle.Completed += onAssetsLoaded;
    }
    private void onAssetsCategoryLoaded(GameObject obj)
    {
        Debug.Log("onAssetsCategoryLoaded");
        SpawnItem(assetKey);
    }
    private void onAssetsLoaded(AsyncOperationHandle<IList<GameObject>> obj)
    {
        Debug.Log("onAssetsLoaded");
    }

    void SpawnItem(string addressableKey)
    {
        Debug.Log("SpawnItem ==> " + addressableKey);
        AsyncOperationHandle<GameObject> asyncLoad = Addressables.InstantiateAsync(addressableKey, Vector3.zero, Quaternion.identity);
        StartCoroutine(progressAsync(asyncLoad));
        asyncLoad.Completed += assetSpawned;
    }
    void SpawnItem(GameObject addressableObj)
    {
        Debug.Log("SpawnItem ==> " + addressableObj);
        AsyncOperationHandle<GameObject> asyncLoad = Addressables.InstantiateAsync(addressableObj);
        StartCoroutine(progressAsync(asyncLoad));
        asyncLoad.Completed += assetSpawned;
    }
    private System.Collections.IEnumerator progressAsync(AsyncOperationHandle<GameObject> asyncOperation)
    {
        float percentLoaded = asyncOperation.PercentComplete;
        while (!asyncOperation.IsDone)
        {
            Debug.Log("Progress = " + percentLoaded + "%");
            yield return 0;
        }
        Debug.Log("Progress Done= " + percentLoaded + "%");
    }
    void assetSpawned(AsyncOperationHandle<GameObject> obj)
    {
        Debug.Log("Instantiate completed ==> " + obj.Status);
    }
}
