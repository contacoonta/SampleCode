
using System.Collections.Generic;
using UnityEngine;

public class loader : MonoBehaviour
{

    private void Start()
    {
        SetMaxRefreshRate();
    }

    private void SetMaxRefreshRate()
    {
        float currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
        float targetRefreshRate = 120f;

        // 지원되는 모든 리프레시 레이트 가져오기
        List<RefreshRate> refreshRates = new List<RefreshRate>();
        foreach (var resolution in Screen.resolutions)
        {
            if (resolution.refreshRateRatio.value > 1)
            {
                refreshRates.Add(resolution.refreshRateRatio);
            }
        }

        // 가장 높은 지원 리프레시 레이트 찾기
        float maxSupportedRefreshRate = 0f;
        foreach (var rate in refreshRates)
        {
            float rateValue = (float)rate.value;
            if (rateValue > maxSupportedRefreshRate)
            {
                maxSupportedRefreshRate = rateValue;
            }
        }

        // 목표 리프레시 레이트와 최대 지원 리프레시 레이트 중 낮은 값 선택
        float newRefreshRate = Mathf.Min(targetRefreshRate, maxSupportedRefreshRate);

        // 새 리프레시 레이트 설정
        if (newRefreshRate > currentRefreshRate)
        {
            RefreshRate closestRefreshRate = refreshRates[0];
            float closestDifference = Mathf.Abs((float)closestRefreshRate.value - newRefreshRate);

            for (int i = 1; i < refreshRates.Count; i++)
            {
                float difference = Mathf.Abs((float)refreshRates[i].value - newRefreshRate);
                if (difference < closestDifference)
                {
                    closestRefreshRate = refreshRates[i];
                    closestDifference = difference;
                }
            }

            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, closestRefreshRate);
            Debug.Log($"Set refresh rate to: {closestRefreshRate.value} Hz");
        }
        else
        {
            Debug.Log($"Current refresh rate is already optimal: {currentRefreshRate} Hz");
        }

        Application.targetFrameRate = Mathf.RoundToInt(newRefreshRate);
        //Debug.Log($"Set target frame rate to: {Application.targetFrameRate}");
    }
}