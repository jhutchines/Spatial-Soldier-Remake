using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
    private string gameId = "3736972";
#elif UNITY_ANDROID
    private string gameId = "3736973";
#endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
    public GameObject shopUI;

    private void Awake()
    {
        if (GameObject.Find("Background").GetComponent<GameManager>().fullVersion) Destroy(gameObject);
        
    }

    void Start()
    {

        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady(myPlacementId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, false);

        shopUI = GameObject.Find("ShopMenu");
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            shopUI.GetComponent<ShopUI>().WatchedAd();
            Advertisement.RemoveListener(this);
        }
        else if (showResult == ShowResult.Skipped)
        {
            myButton.transform.GetChild(0).GetComponent<Text>().text = "Show ad skipped.";
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            myButton.transform.GetChild(0).GetComponent<Text>().text = "Show ad failed.";
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        myButton.transform.GetChild(0).GetComponent<Text>().text = message;
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    public void GameEnd()
    {
        Advertisement.RemoveListener(this);
    }
}