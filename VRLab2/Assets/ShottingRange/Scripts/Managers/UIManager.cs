using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image Lifebar;
    public TextMeshProUGUI life;

    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    private void UpdateHP()
    {
        Tuple<int, int> hpData = gm.GetPlayerHPData();

        life.text = hpData.Item1 + " / " + hpData.Item2;
        Lifebar.fillAmount = Mathf.Clamp((float)hpData.Item1 / hpData.Item2, 0f, 1f);
    }
}
