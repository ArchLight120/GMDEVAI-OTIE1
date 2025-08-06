using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for Button

public class TankManager : MonoBehaviour
{
    public List<GameObject> tanks;
    public TextMeshProUGUI tankNameDisplay;

    void Start()
    {
        SelectTank(0); // Default to first tank on start
    }

    public void SelectTank(int index)
    {
        if (index < 0 || index >= tanks.Count) return;

        for (int i = 0; i < tanks.Count; i++)
        {
            var pathFollower = tanks[i].GetComponent<FollowPath>();
            pathFollower.enabled = (i == index);
        }

        if (tankNameDisplay != null)
        {
            tankNameDisplay.text = "Current Tank: " + tanks[index].name;
        }
    }
}
