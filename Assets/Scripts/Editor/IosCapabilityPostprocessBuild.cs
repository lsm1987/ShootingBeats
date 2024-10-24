using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

/// <summary>
/// iOS 빌드 시 Capability 지정.<br />
/// <see href="https://discussions.unity.com/t/upcoming-game-center-entitlement-requirement-by-apple/925741"/> 참고
/// </summary>
public class IosCapabilityPostprocessBuild : IPostprocessBuildWithReport
{
    private static readonly string entitlementsTemplate =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
    <dict>
    </dict>
</plist>"; 

    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.iOS)
        {
            WriteCapailityEntitlements(report.summary.outputPath);
        }
    }

    private static void WriteCapailityEntitlements(string buildOutputPath)
    {
        Debug.Log($"BuildOutputPath: {buildOutputPath}");
        
        string projPath = PBXProject.GetPBXProjectPath(buildOutputPath);
        Debug.Log($"PBXProjectPath: {projPath}");
        
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        var mainTarget = proj.GetUnityMainTargetGuid();

        // Get path to entitlement file (if one was created by Unity) using the Main Target. If the file doesn't exist, we make our own
        string entitlementPath = proj.GetEntitlementFilePathForTarget(mainTarget) ?? $"{buildOutputPath}/Entitlements.entitlements";
        Debug.Log($"EntitlementFilePath: {entitlementPath}");
        
        // Create a new entitlement file if one doesn't exist and populate it with the template
        if(!File.Exists(entitlementPath))
        {
            File.WriteAllText(entitlementPath, entitlementsTemplate);
        }
        
        PlistDocument entitlementsDoc = new PlistDocument();
        entitlementsDoc.ReadFromString(File.ReadAllText(entitlementPath));
        
        // Add the Game Center capability to the root dict of the entitlements file
        PlistElementDict entitlementDict = entitlementsDoc.root;
        entitlementDict.SetBoolean("com.apple.developer.game-center", true);
        File.WriteAllText(entitlementPath, entitlementsDoc.WriteToString());
        
        proj.SetBuildProperty(mainTarget, "CODE_SIGN_ENTITLEMENTS", entitlementPath);
        proj.WriteToFile(projPath);
        
        Debug.Log("WriteCapailityEntitlements Done");
    }
}