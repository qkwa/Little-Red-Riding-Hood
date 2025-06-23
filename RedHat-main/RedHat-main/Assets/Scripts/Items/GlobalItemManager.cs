using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalItemManager : MonoBehaviour
{
    public static GlobalItemManager Instance;

    [Header("Settings")]
    [SerializeField] private float defaultRespawnTime = 30f;
    [SerializeField] private Vector2 randomTimeOffset = new Vector2(-5f, 5f);

    private Dictionary<InteractItem, ItemRespawnData> items = new Dictionary<InteractItem, ItemRespawnData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterItem(InteractItem item, Vector3 originalPosition, Transform customSpawnPoint = null)
    {
        if (!items.ContainsKey(item))
        {
            items.Add(item, new ItemRespawnData(originalPosition, customSpawnPoint));
            item.OnPickup += () => StartCoroutine(RespawnItem(item));
        }
    }

    private IEnumerator RespawnItem(InteractItem item)
    {
        if (!items.TryGetValue(item, out var data)) yield break;

        float delay = defaultRespawnTime + Random.Range(randomTimeOffset.x, randomTimeOffset.y);
        yield return new WaitForSeconds(delay);

        if (item != null)
        {
            item.transform.position = data.customSpawnPoint != null ?
                data.customSpawnPoint.position : data.originalPosition;

            item.gameObject.SetActive(true);

            if (item is IResettable resettable)
                resettable.ResetState();
        }
    }

    private class ItemRespawnData
    {
        public Vector3 originalPosition;
        public Transform customSpawnPoint;

        public ItemRespawnData(Vector3 position, Transform spawnPoint)
        {
            originalPosition = position;
            customSpawnPoint = spawnPoint;
        }
    }
}