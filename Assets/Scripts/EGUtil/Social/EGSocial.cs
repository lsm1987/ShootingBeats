using UnityEngine;
using System;

public static class EGSocial
{
    private static IEGSocialLogic _Logic { get; set; }

    /// <summary>
    /// 소셜 기능 활성화
    /// </summary>
    public static void Activate()
    {
        if (_Logic == null)
        {
            _Logic = CreateLogic();
        }

        _Logic.Activate();
    }

    private static IEGSocialLogic CreateLogic()
    {
#if UNITY_ANDROID
        return new EGAndroidSocialLogic();
#elif UNITY_IOS
        return new EGIosSocialLogic();
#else
        return new EGGeneralSocialLogic();
#endif
    }

    /// <summary>
    /// 로그인 시도 중
    /// </summary>
    public static bool _IsAuthenticating
    {
        get;
        private set;
    }

    /// <summary>
    /// 로그인 되어있는가?
    /// </summary>
    public static bool _IsAuthenticated
    {
        get
        {
            return Social.Active.localUser.authenticated;
        }
    }

    /// <summary>
    /// 로그인 수행
    /// </summary>
    public static void Authenticate(Action<bool> callback)
    {
        if (_IsAuthenticated || _IsAuthenticating)
        {
            Debug.LogWarning("[EGSocial] Ignoring repeated call to Authenticate().");
            return;
        }

        _IsAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            _IsAuthenticating = false;
            if (success)
            {
                Debug.Log("[EGSocial] Login successful!");
            }
            else
            {
                Debug.LogWarning("[EGSocial] Failed to sign in.");
            }

            // 결과 발생시 호출부에서 실행할 내용
            if (callback != null)
            {
                callback(success);
            }
        });
    }

    /// <summary>
    /// 로그아웃 수행
    /// </summary>
    public static void SignOut()
    {
        _Logic.SignOut();
    }

    public static void ShowAchievementsUI()
    {
        if (!_IsAuthenticated)
        {
            Debug.LogWarning("[EGSocial] ShowAchievementsUI - Not signed in.");
        }

        Social.ShowAchievementsUI();
    }

    public static void ShowLeaderboardUI(string leaderboardID)
    {
        if (!_IsAuthenticated)
        {
            Debug.LogWarning("[EGSocial] ShowLeaderboardUI - Not signed in.");
        }

        _Logic.ShowLeaderboardUI(leaderboardID);
    }

    public static void ReportScore(long score, string board)
    {
        if (!_IsAuthenticated)
        {
            Debug.LogWarning("[EGSocial] ReportScore - Not signed in.");
        }

        Social.ReportScore(score, board, success =>
        {
            if (success)
            {
                Debug.Log(string.Format("[EGSocial] Score reported successfully. board: {0}, score: {1}"
                    , board, score));
            }
            else
            {
                Debug.LogError(string.Format("[EGSocial] Failed to report score! board: {0}, score: {1}"
                    , board, score));
            }
        });
    }

    public static void ReportProgress(string achievementID, double progress)
    {
        if (!_IsAuthenticated)
        {
            Debug.LogWarning("[EGSocial] ReportProgress - Not signed in.");
        }

        Social.ReportProgress(achievementID, progress, success =>
        {
            if (success)
            {
                Debug.Log(string.Format("[EGSocial] Achievement reported successfully. achievementID: {0}, progress: {1}"
                    , achievementID, progress));
            }
            else
            {
                Debug.LogError(string.Format("[EGSocial] Failed to report achievement! achievementID: {0}, progress: {1}"
                    , achievementID, progress));
            }
        });
    }
}