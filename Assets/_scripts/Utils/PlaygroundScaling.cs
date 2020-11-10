using UnityEngine;
using System.Collections;

public class PlaygroundScaling : MonoBehaviour 
{
    private Camera mainCamera;

    [Header("RENDERER CONTAINING THE FIELD DIMENSIONS")]
	[SerializeField] private SpriteRenderer background;

    [Header("SET ASPECT RATIO TO BACKGROUND RATION")]
    [SerializeField] private bool rescale;

	private void Awake ()
	{
        mainCamera = Camera.main;

		float bgWidth = background.size.x;
		float bgHeight = background.size.y;

        float bgAspect = bgWidth / bgHeight;

        if (rescale)
            mainCamera.aspect = bgAspect;

        float screenHeight = mainCamera.orthographicSize * 2;
		float screenWidth = screenHeight * mainCamera.aspect;

        //float scaleRatioX = screenWidth/bgWidth;
        //float scaleRatioY = screenHeight/bgHeight;


        if (bgAspect < mainCamera.aspect)
        {
            mainCamera.orthographicSize = bgHeight / 2;
        }
        else
        {
            mainCamera.orthographicSize = bgWidth / mainCamera.aspect / 2;
        }


		//transform.localScale = new Vector3(scaleRatioX, scaleRatioY, 1);
	}

}
