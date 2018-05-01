using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TheWorld;
public class Game : MonoBehaviour {
    WorldGenerator generator = new WorldGenerator();
    public GameObject prefab;
    bool[,] map;
    void Awake()
    {
        generator.run(out map);
    }
	// Use this for initialization
	void Start () {
        for(int i = 0; i < 501; i++)for(int j = 0; j < 501; j++)
        {
                if(map[i,j])
                {
                    GameObject.Instantiate(prefab).transform.position = new Vector3(i, j, 0);
                }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
