using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private bool isCollected;
    [SerializeField] private GameObject collectible;
    [SerializeField] private AudioClip sfx_onNewLocationDiscover;

    [SerializeField] private ScriptObj_AreaId _areaId;
    [SerializeField] private string id;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && !isCollected)
            {
                StartCoroutine(OnCollected());
                isCollected = true;
                Manager_SFXPlayer.instance.PlaySFXClip(sfx_onNewLocationDiscover, transform, 1f, mixerGroup: Manager_AudioMixer.instance.mixer_sfx);
                DataPersistenceManager.instance.SaveGame();
            }
        }
    }

    private IEnumerator OnCollected()
    {
        LeanTween.scale(collectible, new Vector3(1.2f, 1.2f, 1.2f), 0.1f);
        LeanTween.scale(collectible, new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setDelay(0.1f);
        LeanTween.scale(collectible, new Vector3(0f, 0f, 0f), 1f).setDelay(0.2f).setEaseOutBounce();
        yield return new WaitForSeconds(1.3f);
        OnFinishCollected();
        
    }

    private void OnFinishCollected()
    {
        collectible.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        // Search up area data based on id
        AreaSet areaSet = data.SearchAreaWithId(_areaId.name);
        SerializableDictionary<string, bool> collectiblesCollected = areaSet.collectiblesCollected;

        collectiblesCollected.TryGetValue(id, out isCollected);
        if (isCollected)
        {
            OnFinishCollected();
        }
    }

    public void SaveData(ref GameData data)
    {
        // Search up area data based on id
        AreaSet areaSet = data.SearchAreaWithId(_areaId.name);
        SerializableDictionary<string, bool> collectiblesCollected = areaSet.collectiblesCollected;

        if (collectiblesCollected.ContainsKey(id))
        {
            collectiblesCollected.Remove(id);
        }
        collectiblesCollected.Add(id, isCollected);
    }
}
