using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SwipeEvents : UnityEvent { }

public class SwipeInput : MonoBehaviour
{
    #region Variables

    private LocalGameManager manager;

	// If the touch is longer than MAX_SWIPE_TIME, dont consider it a swipe
	private const float MAX_SWIPE_TIME = 0.5f;
	// Factor of the screen width that is considered a swipe
	private const float MIN_SWIPE_DISTANCE = 0.02f;
	// Duration between taps to be considered as a double tap
	private const float MAX_DOUBLE_TAP_TIME = 0.1f;



	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;

	public SwipeEvents OnSwipeRight, OnSwipeLeft, OnSwipeUp, OnSwipeDown;
	public SwipeEvents OnSingleTap, OnDoubleTap;

	// Whether or not to look for keyboard inputs
	public bool debugWithArrowKeys = true;

	//Cached variables
	private Vector2 startPos, endPos, swipe;
	private bool swiped = false;
	private Touch t;

	private int tapCount = 0;
	private float doubleTapTimer = 0;

    #endregion

    private void Start()
    {
		manager = LocalGameManager.Instance;
    }
    public void Update()
	{
		//Dont check inputs when dead or not running
		if (!manager.isGameStarted || manager.isDead)
			return;

		//Reset swipe status to prevent multiple event triggers
		swipedRight = false;
		swipedLeft = false;
		swipedUp = false;
		swipedDown = false;

		if (Input.touches.Length > 0)
		{
			t = Input.GetTouch(0);

			if (t.phase == TouchPhase.Began)
			{
				//Look for single taps when in a boss fight
				if (t.position.y > Screen.height / 2f && manager.bossActive)
				{
					OnSingleTap.Invoke();
					return;
				}

				startPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);
				tapCount++;
			}
			else if(t.phase == TouchPhase.Moved && !swiped)
			{
				endPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);
				swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				// Too short swipe
				if (swipe.magnitude < MIN_SWIPE_DISTANCE)
					return;

				swiped = true;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{
					// Horizontal swipe
					if (swipe.x > 0)
					{
						swipedRight = true;
						OnSwipeRight.Invoke();
					}
					else
					{
						swipedLeft = true;
						OnSwipeLeft.Invoke();
					}
				}
				else
				{
					// Vertical swipe
					if (swipe.y > 0)
					{
						swipedUp = true;
						OnSwipeUp.Invoke();
					}
					else
					{
						swipedDown = true;
						OnSwipeDown.Invoke();
					}
				}
			}
			else if (t.phase == TouchPhase.Ended)
				swiped = false;
		}

		//Too long for double tap
		if (doubleTapTimer > MAX_DOUBLE_TAP_TIME)
		{
			doubleTapTimer = 0f;
			tapCount = 0;
		}
		//Double tap
		else if (tapCount >= 2)
		{
			doubleTapTimer = 0.0f;
			tapCount = 0;
			OnDoubleTap.Invoke();
		}
		//Increase double tap timer
		else if (tapCount > 0)
			doubleTapTimer += Time.deltaTime;

		//If debugging, look for keyboard inputs
		if (debugWithArrowKeys)
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
				OnSwipeRight.Invoke();
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				OnSwipeLeft.Invoke();
			if (Input.GetKeyDown(KeyCode.UpArrow))
				OnSwipeUp.Invoke();
			if (Input.GetKeyDown(KeyCode.DownArrow))
				OnSwipeDown.Invoke();
			if (Input.GetKeyDown(KeyCode.E))
				OnSingleTap.Invoke();
			if (Input.GetKeyDown(KeyCode.Space))
				OnDoubleTap.Invoke();
		}
	}
}