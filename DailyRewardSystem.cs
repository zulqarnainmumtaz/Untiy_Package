using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardSystem : MonoBehaviour
{
    public GameObject claimButton;
    public GameObject claimedButton;
    public Text timeRemainingText;
    public float rewardDelay = 24f; // Time delay between rewards in hours
    public int assignedDay = 1; // Day (reward index) assigned in the Inspector

    private DateTime lastClaimTime;
    private bool isUpdatingTime = false;
    public DailyRewardActivation DRA;
    //public GameObject[] AllRewards;


    private void Start()
    {
        if (!this.enabled) return;
            LoadLastClaimTime();
        UpdateUI();
        StartCoroutine(UpdateClaimTimeContinuously());
        // if (PlayerPrefs.HasKey("Currentday"))
        // {
        //     AllRewards[0].transform.GetComponent<DailyRewardSystem>().enabled = true;
        // }
        //if (!PlayerPrefs.HasKey("Currentday"))
        //{
        //    PlayerPrefs.SetInt("Currentday", 0);
        //    //AllRewards[PlayerPrefs.GetInt("Currentday")].transform.GetComponent<DailyRewardSystem>().enabled = true;
        //}
    }
    private void OnApplicationQuit()
    {
        if (!this.enabled) return;
        SaveLastClaimTime();
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!this.enabled) return;
        if (hasFocus)
        {
            LoadLastClaimTime();
            UpdateUI();
        }
    }
   

    private void Update()
    {
        if (!this.enabled) return;
        if (isUpdatingTime)
        {
            UpdateTimeRemaining();
        }
    }

    public void ClaimReward()
    {
        if (!this.enabled) return;
        if (CanClaimReward())
        {
            GiveReward();
            lastClaimTime = DateTime.Now;
            SaveLastClaimTime();
            UpdateTimeRemaining();
            StartCoroutine(UpdateClaimTimeContinuously());
        }
        else
        {
            // Update time remaining immediately
            UpdateTimeRemaining();
            StartCoroutine(UpdateClaimTimeContinuously());
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!this.enabled) return;
        bool canClaim = CanClaimReward();
        claimButton.SetActive(canClaim);
        claimedButton.SetActive(!canClaim);

        if (canClaim)
        {
            timeRemainingText.gameObject.SetActive(false);
        }
        else
        {
            timeRemainingText.gameObject.SetActive(true);
            TimeSpan timeRemaining = lastClaimTime.AddHours(rewardDelay) - DateTime.Now;
            timeRemainingText.text = "" + FormatTime(timeRemaining);
        }
    }
    private bool CanClaimReward()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimTime;
        return timeSinceLastClaim.TotalHours >= rewardDelay;
    }

    private void GiveReward()
    {
       PlayerPrefs.SetInt("CurrentDay",PlayerPrefs.GetInt("CurrentDay")+1);
        for(int i=0;i<DRA.AllRewards.Length;i++){
              DRA.AllRewards[i].transform.GetComponent<DailyRewardSystem>().enabled =false;
            }
           DRA.AllRewards[PlayerPrefs.GetInt("CurrentDay")].transform.GetComponent<DailyRewardSystem>().enabled =true;
      
    }

    private void UpdateTimeRemaining()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimTime;
        TimeSpan timeRemaining = TimeSpan.FromHours(rewardDelay) - timeSinceLastClaim;

        if (timeRemaining.TotalSeconds <= 0)
        {
            timeRemaining = TimeSpan.Zero;
           
            


        }

        timeRemainingText.text = "" + FormatTime(timeRemaining);

        // Update claim button and claimed button visibility based on time remaining
        if (CanClaimReward())
        {
            claimButton.SetActive(true);
            claimedButton.SetActive(false);
        }
        else
        {
            claimButton.SetActive(false);
            claimedButton.SetActive(true);
          
        }
    }

    private System.Collections.IEnumerator UpdateClaimTimeContinuously()
    {
        isUpdatingTime = true;
        while (!CanClaimReward())
        {
            UpdateTimeRemaining();
            yield return new WaitForSeconds(1f); // Update time every second
        }
        isUpdatingTime = false;
        UpdateUI();
    }

    private void LoadLastClaimTime()
    {
        if (PlayerPrefs.HasKey("LastClaimTime"))
        {
            lastClaimTime = DateTime.Parse(PlayerPrefs.GetString("LastClaimTime"));
        }
        else
        {
            lastClaimTime = DateTime.MinValue;
        }
    }


    private void SaveLastClaimTime()
    {
        PlayerPrefs.SetString("LastClaimTime", lastClaimTime.ToString());
    }

    private string FormatTime(TimeSpan timeSpan)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}