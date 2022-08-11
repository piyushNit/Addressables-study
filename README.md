# Addressables-study

Project configurations:
Unity : 2020.3.29f1
Addressable : 1.18.19
URP : 10.8.1

I am investigating how to load external asset bundle using Addressables.

Basically, The ideas is that I wanted to keep multiple versions of the assets on the server and client wants to decide which version of asset it wants to download.
So that later I can automate this process and configure the asset version when the application initially connect with the server.

In this demo I am using Google cloud storage.
On the server I have put the data into following paths.

For windows
Version1 : https://storage.googleapis.com/petpilot_addressables/petpilot_v1/ServerData/StandaloneWindows64
Version2 : https://storage.googleapis.com/petpilot_addressables/petpilot_v2/ServerData/StandaloneWindows64

For Android
Version1 : https://storage.googleapis.com/petpilot_addressables/petpilot_v1/ServerData/Android
Version2 : https://storage.googleapis.com/petpilot_addressables/petpilot_v2/ServerData/Android

for using google cloud store this tutorial was very helpful for me.
https://mikecbauervision.medium.com/unity-addressables-firebase-google-cloud-storage-632191b86b9c

Asset bundles were built fromt the saperate project.
And loading the assets into saperate project.

For more details please follow the below link:
https://forum.unity.com/threads/generating-assetbundles-in-a-separate-project-for-multiple-platforms.567151/#post-5029094

Anyone wants to try this out, just download the project and run the SampleScene.
Then select the main camera, there you can find the 'AddressableExternalLoad' component.
Select the platform and version the run the scene.

Known issue:
On windows sometimes I am getting an error while downloading the assets, its pointing the wrong server path.
I didn't dig into that. After clearing the cache it worked for me.
Also I didn't checked this on to the real android device.


Please let me know if you have any comments on this.

