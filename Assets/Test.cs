using UnityEngine;
using System.Collections;
using KLFrame;
public class Test : MonoBehaviour {
    AudioSource clip;
    // Use this for initialization
    void Start () {
        //AudioUtility.PlaySound("Intro").OnStopPlay=(sender,arg)=> { Debug.Log("on stop"+arg.ClipName); };
        StopCoroutine(StartCoroutine(Cortest()));
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Cortest() {
        yield return new WaitForSeconds(3);
        Debug.Log("sssssssss");
    }

    public void PlaySound() {
        float x = Random.Range(0,10);
        float y = Random.Range(0,10);
        AudioUtility.PlaySoundAtLocation("Intro",new Vector3(x,y,0));
     //   AudioUtility.PlaySoundAtLocation("Intro",Camera.main.transform.position); 
    }
}
