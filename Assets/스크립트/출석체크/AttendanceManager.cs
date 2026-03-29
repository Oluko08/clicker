using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class AttendanceManager : MonoBehaviour
{
    [Header("🔗 시스템 연결")]
    private SystemZamsu scoreSystem;
    public GameObject attendancePanel; // 출석창 부모 오브젝트

    [Header("📅 출석 데이터")]
    public List<AttendanceData> weeklyRewards; // 7개의 데이터 연결
    public List<AttendanceSlotUI> slots; // 7개의 슬롯 UI 연결

    private int currentStep = 0; // 현재 몇 번째 보상을 받을 차례인지 (0~6)
    private bool hasCheckedToday = false;

    private void Awake()
    {
        scoreSystem = FindAnyObjectByType<SystemZamsu>();
        LoadAttendanceData();
    }

    private void Start()
    {
        CheckNewDay();
        // 오늘 보상을 아직 안 받았다면 창을 띄움
        if (!hasCheckedToday)
        {
            OpenPanel();
        }
    }

    private void CheckNewDay()
    {
        string lastDateStr = PlayerPrefs.GetString("LastCheckDate", "");
        string todayDateStr = DateTime.Now.ToString("yyyyMMdd");

        if (lastDateStr != todayDateStr)
        {
            hasCheckedToday = false;
        }
        else
        {
            hasCheckedToday = true;
        }
    }

    private void LoadAttendanceData()
    {
        currentStep = PlayerPrefs.GetInt("AttendanceStep", 0);
    }

    public void OpenPanel()
    {
        attendancePanel.SetActive(true);
        UpdateUI();
    }

    public void ClosePanel()
    {
        attendancePanel.SetActive(false);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            // 이미 받은 날인지, 오늘 받을 날인지 표시
            bool isAlreadyReceived = (i < currentStep); // 이 시스템은 보상을 받아야 step이 올라감
            slots[i].SetState(weeklyRewards[i], i == currentStep, isAlreadyReceived);
        }
    }

    // 버튼 클릭 시 호출 (i는 0~6)
    public void OnClickReward(int index)
    {
        if (index != currentStep || hasCheckedToday) return;

        StartCoroutine(ClaimProcess(index));
    }

    private IEnumerator ClaimProcess(int index)
    {
        hasCheckedToday = true;
        AttendanceData data = weeklyRewards[index];

        // 1. 도장 애니메이션 실행
        yield return StartCoroutine(slots[index].StampAnimation());

        // 2. 실제 보상 지급
        if (data.rewardName == "Item10") scoreSystem.item10Count += data.amount;
        else if (data.rewardName == "Item100") scoreSystem.item100Count += data.amount;
        
        scoreSystem.SaveGame();

        // 3. 데이터 저장
        PlayerPrefs.SetString("LastCheckDate", DateTime.Now.ToString("yyyyMMdd"));
        
        // 7일차면 다시 0으로 루프, 아니면 다음 칸으로
        currentStep = (currentStep + 1) % 7;
        PlayerPrefs.SetInt("AttendanceStep", currentStep);
        PlayerPrefs.Save();

        Debug.Log($"{data.rewardDisplayName} 획득!");
        UpdateUI();
    }
}