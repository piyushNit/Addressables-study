using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddressableUIDownload : MonoBehaviour
{
    public AddressablesExternalLoad addressableExternalLoad;

    public Toggle windowsToggle;
    public Toggle androidToggle;

    public Toggle ver1Toggle;
    public Toggle ver2Toggle;

    public Text text;

    public void StartDownload()
    {
        if(addressableExternalLoad.initsuccess)
        {
            string catalogPath = addressableExternalLoad.GetCatalogPath(windowsToggle.isOn ? Platform.Windows : Platform.Android, ver1Toggle.isOn ? AssetVersion.Ver_1 : AssetVersion.Ver_2);
            addressableExternalLoad.loadCatalog(catalogPath);
        }
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLogs;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLogs;
    }
    void HandleLogs(string logString, string stackTrace, LogType type)
    {
        text.text = logString;
    }

}
