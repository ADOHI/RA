using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : Singleton<CharacterSpawner>
{
    public GameObject characterPrefab;
    public GameObject mirrorImagePrefab;
    public Vector3 characterSpawnPosition;
    public Vector3 mirrorImageSpawnPosition;
    public float spawnDistance = 3f;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        Transform[] childList = GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
        var obj = Instantiate(characterPrefab, characterSpawnPosition, Quaternion.identity, transform);
        Instantiate(mirrorImagePrefab, characterSpawnPosition - obj.transform.forward * spawnDistance, Quaternion.identity, transform);
    }
}
