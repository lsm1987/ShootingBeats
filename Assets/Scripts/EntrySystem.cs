using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;

public class EntrySystem : MonoBehaviour
{
    private async void Awake()
    {
        await LoadEssentialStringTables();

        SceneManager.LoadScene(SceneName._Title);
    }

    private async Task LoadEssentialStringTables()
    {
        Debug.Log("[Entry] LoadEssentialStringTables");
        
        TableReference[] tableRefs = new TableReference[] { StringTableName._ui };

        foreach (TableReference tableRef in tableRefs)
        {
            var table = await LocalizationSettings.StringDatabase.GetTableAsync(tableRef).Task;
            if (table == null)
            {
                Debug.LogError("[Entry] Failed to load essential string table. Name:" + tableRef.TableCollectionName);
            }
        }
    }
}
