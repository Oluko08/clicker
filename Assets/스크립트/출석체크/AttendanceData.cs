using UnityEngine;

[CreateAssetMenu(fileName = "NewAttendance", menuName = "TrendSystem/Attendance")]
public class AttendanceData : ScriptableObject
{
    public int dayIndex; 
    public string rewardName; // "Item10" 또는 "Item100"
    public int amount; 
    public string rewardDisplayName; 
}