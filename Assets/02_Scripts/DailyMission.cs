using TMPro;
using UnityEngine;

public class DailyMission : MonoBehaviour
{
    [SerializeField] TMP_Text dailyMissionTxt;
    private void OnEnable()
    {
        dailyMissionTxt = GetComponent<TMP_Text>();
        GameMgr.dailyMissionText = dailyMissionTxt;
        GameMgr.Instance.StartDay(1);

    }
}
