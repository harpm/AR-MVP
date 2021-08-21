using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ArHelper : MonoBehaviour
{
    // Core Variable
    [SerializeField]
    private VideoPlayer player;

    [SerializeField]
    private VideoClip onFindPlane;

    [SerializeField]
    private VideoClip onPlaceStuff;

    // Utilities
    private PlayerStatus status;

    // Stored variables
    private ARPlaneManager arPlaneManager;

    // Start is called before the first frame update
    void Start()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ManageClips()
    {
        if (arPlaneManager.trackables.count == 0 && status != PlayerStatus.Plane)
        {
            PlayVideo(onFindPlane);
            status = PlayerStatus.Plane;
        }
        else if (ArSceneManager.Instance.curStuff != null && status != PlayerStatus.Place)
        {
            PlayVideo(onPlaceStuff);
            status = PlayerStatus.Place;
        }
        else if (arPlaneManager.trackables.count != 0 && ArSceneManager.Instance.curStuff == null && status != PlayerStatus.None)
        {
            StopPlayer();
        }
    }

    private void PlayVideo(VideoClip clip)
    {
        player.gameObject.SetActive(true);
        player.enabled = true;
        player.clip = clip;
        player.isLooping = true;
        player.Play();
    }

    private void StopPlayer()
    {
        player.clip = default;
        player.Stop();
        player.enabled = false;
        player.gameObject.SetActive(false);
        status = PlayerStatus.None;
    }

    private enum PlayerStatus
    {
        None,
        Plane,
        Place
    }
}
