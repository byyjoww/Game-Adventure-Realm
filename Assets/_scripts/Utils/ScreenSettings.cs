using UnityEngine;

public class ScreenSettings
{
	// Based on safe area of iPhoneX
	public static float DesignResolutionScreenWidth = 2202;
	public static float DesignResolutionScreenHeight = 1125;
	public static float MaxScreenAspectRatio = DesignResolutionScreenWidth / DesignResolutionScreenHeight;

	public static float NotchWidthPercentage => 1f - Screen.safeArea.width / Screen.width;
	public static ScreenOrientation Orientation => Screen.orientation;

	private static Vector2 _screenSize = Vector2.zero;
	
	public static float Width { get { return GetDesignResolutionSize().x; } }
	public static float Height { get { return GetDesignResolutionSize().y; } }
	public static float Scale
	{
		get
		{
			if (IsExceededMaxScreenAspectRatio())
			{
				return Screen.height / DesignResolutionScreenHeight;
			}
			return Screen.width / DesignResolutionScreenWidth;
		}
	}

	
	public static float GetScreenAspectRatio() {
		return (float) Screen.width / Screen.height;
	}

	public static Vector2 GetDesignResolutionSize()
	{
		if (_screenSize == Vector2.zero)
		{
			float factorRatio = IsExceededMaxScreenAspectRatio() ? MaxScreenAspectRatio : GetScreenAspectRatio();
			_screenSize = new Vector2(DesignResolutionScreenWidth, DesignResolutionScreenWidth / factorRatio);
		}
		return _screenSize;
	}

	public static float GetDesignResolutionRatio()
	{
		var size = GetDesignResolutionSize();
		return size.y / size.x;
	}
	
	public static float GetExceededWidth()
	{
		if (!IsExceededMaxScreenAspectRatio())
		{
			return 0.0f;
		}
		
		float ratio = GetScreenAspectRatio();
		return ratio * DesignResolutionScreenHeight - DesignResolutionScreenWidth;
	}
	
	public static bool IsExceededMaxScreenAspectRatio()
	{
		return Screen.width * DesignResolutionScreenHeight > Screen.height * DesignResolutionScreenWidth;
	}
}