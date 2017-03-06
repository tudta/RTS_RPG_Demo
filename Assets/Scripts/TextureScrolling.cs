using UnityEngine;
using System.Collections;

public class TextureScrolling : MonoBehaviour {
    [SerializeField] private Material mat;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private float scrollSpeed = 0.0f;
    [SerializeField] private float scrollSpeedRngMin = 0.0f;
	[SerializeField] private float scrollSpeedRngMax = 1.0f;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        offset += new Vector2(Random.Range(scrollSpeedRngMin, scrollSpeedRngMax) * Time.deltaTime, Random.Range(scrollSpeedRngMin, scrollSpeedRngMax) * Time.deltaTime);
        mat.SetTextureOffset("_MainTex", offset);
        //mat.SetTextureOffset("_MainTex", new Vector2 (mat.mainTextureOffset.x + Random.Range (0.0f, 1.0f), mat.mainTextureOffset.y + Random.Range(0.0f, 1.0f)));
    }
}
