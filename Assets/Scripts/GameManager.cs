using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public LevelBuilder builder;

    //public Animator animat;

	// Use this for initialization
	void Awake () {
	    if(instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        builder = GetComponent<LevelBuilder>();
        InitScript();
    }
	
	
	void InitScript () {
        builder.StartLevel();
	}
}
