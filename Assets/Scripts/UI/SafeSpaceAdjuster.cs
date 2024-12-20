using UnityEngine;
using Utilities;

namespace UI
{
    public class SafeSpaceAdjuster : MonoBehaviour
    {
        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }
        public float UnitSize { get; private set; }
        public bool IsInitialized { get; private set; }

        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform topPanel;

        public void Initialize()
        {
            CanvasHeight = canvas.GetComponent<RectTransform>().sizeDelta.y;
            CanvasWidth = canvas.GetComponent<RectTransform>().sizeDelta.x;
            UnitSize = canvas.referencePixelsPerUnit * (CanvasHeight / Constants.DefaultScreenHeight);
            
            topPanel.anchoredPosition += Vector2.down * ScreenSafeAreaTopDifferenceInPixels() / 2;

            IsInitialized = true;
        }
        
        private float SafeAreaChangeInUnits()
        {
            return ScreenSafeAreaTopDifferenceInPixels() * 0.5f / UnitSize;
        }

        private float ScreenSafeAreaTopDifferenceInPixels()
        {
            var screenSafeAreaTopDifferenceInPixels = Mathf.Abs(Screen.safeArea.max.y - Screen.height);
            if (screenSafeAreaTopDifferenceInPixels > 0f)
            {
                screenSafeAreaTopDifferenceInPixels += UnitSize / 2f; 
            }
        
            return screenSafeAreaTopDifferenceInPixels;
        }
    }
}