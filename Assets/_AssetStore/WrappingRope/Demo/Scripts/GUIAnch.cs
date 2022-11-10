using UnityEngine;
using WrappingRopeLibrary.Scripts;
using WrappingRopeLibrary.Enums;

public class GUIAnch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        _ropeEntity = Rope.GetComponent<Rope>();
        _ropeLength = _ropeEntity.GetRopeLength();
    }
	
	// Update is called once per frame
	void Update () {
        flags[0] = false;
        flags[1] = false;
    }


    void FixedUpdate()
    {
        //_update = true;
        if (_ropeEntity == null)
            return;
        if (flags[0] == true)
        {
            _ropeLength -= 1f * Time.fixedDeltaTime;
        }
        else
        if (flags[1] == true)
        {
            _ropeLength += 1f * Time.fixedDeltaTime;
        }
        _ropeEntity.SetRopeLength(_ropeLength);
    }

    void OnGUI()
    {
        TestControls();
    }
 
    public GameObject Rope;
    private Rope _ropeEntity;
    private float _ropeLength;  
 

    private bool[] flags = new bool[] { false, false };

    private void TestControls()
    {
        if (_ropeEntity == null)
            return;

        GUILayout.BeginArea(new Rect(10, 10, 200, 300));
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal("box");
        if (GUILayout.RepeatButton("Cut the rope"))
        {
            flags[0] = flags[0] | true;
            flags[1] = false;
        }

        if (GUILayout.RepeatButton("Expand the rope"))
        {
            flags[0] = false;
            flags[1] = flags[1] | true;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
