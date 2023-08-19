using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardActivation : MonoBehaviour
{
    public GameObject[] AllRewards;
    //public DailyRewardSystem DRS;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("CurrentDay")==0){
            AllRewards[0].transform.GetComponent<DailyRewardSystem>().enabled =true;
        }
        else
        {
            for(int i=0;i<AllRewards.Length;i++){
               AllRewards[i].transform.GetComponent<DailyRewardSystem>().enabled =false;
            }
            AllRewards[PlayerPrefs.GetInt("CurrentDay")].transform.GetComponent<DailyRewardSystem>().enabled =true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
