using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PCLoveGame.UI
{
    public static class UiFactory
    {
        private static Font _cachedFont;

        public static Canvas CreateCanvas(Transform parent)
        {
            GameObject canvasObject = new GameObject("RuntimeCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(parent, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            return canvas;
        }

        public static void EnsureEventSystem()
        {
            if (UnityEngine.Object.FindObjectOfType<EventSystem>() != null)
            {
                return;
            }

            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        public static Image CreatePanel(Transform parent, string objectName, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject panelObject = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(parent, false);

            RectTransform rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;

            Image image = panelObject.GetComponent<Image>();
            image.color = color;
            return image;
        }

        public static Text CreateText(Transform parent, string textValue, int fontSize, TextAnchor alignment, Color color)
        {
            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            Text text = textObject.GetComponent<Text>();
            text.font = GetFont();
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.text = textValue;
            return text;
        }

        public static Button CreateButton(Transform parent, string label, Action onClick)
        {
            GameObject buttonObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            Image background = buttonObject.GetComponent<Image>();
            background.color = new Color(0.16f, 0.18f, 0.22f, 0.95f);

            Button button = buttonObject.GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = background.color;
            colors.highlightedColor = new Color(0.23f, 0.28f, 0.35f, 1f);
            colors.pressedColor = new Color(0.13f, 0.16f, 0.2f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            Text text = CreateText(buttonObject.transform, label, 22, TextAnchor.MiddleCenter, Color.white);
            Stretch(text.rectTransform);

            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick.Invoke());
            }

            LayoutElement layoutElement = buttonObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 54f;

            return button;
        }

        public static InputField CreateInputField(Transform parent, string placeholderText)
        {
            GameObject rootObject = new GameObject("InputField", typeof(RectTransform), typeof(Image), typeof(InputField));
            rootObject.transform.SetParent(parent, false);

            Image background = rootObject.GetComponent<Image>();
            background.color = new Color(0.09f, 0.11f, 0.15f, 1f);

            LayoutElement layoutElement = rootObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 54f;

            InputField inputField = rootObject.GetComponent<InputField>();

            Text placeholder = CreateText(rootObject.transform, placeholderText, 20, TextAnchor.MiddleLeft, new Color(1f, 1f, 1f, 0.35f));
            Text content = CreateText(rootObject.transform, string.Empty, 20, TextAnchor.MiddleLeft, Color.white);

            SetTextPadding(placeholder.rectTransform, 12f);
            SetTextPadding(content.rectTransform, 12f);

            inputField.placeholder = placeholder;
            inputField.textComponent = content;
            return inputField;
        }

        public static VerticalLayoutGroup CreateVerticalLayout(Transform parent, float spacing, TextAnchor alignment)
        {
            VerticalLayoutGroup layout = parent.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = alignment;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return layout;
        }

        public static HorizontalLayoutGroup CreateHorizontalLayout(Transform parent, float spacing, TextAnchor alignment)
        {
            HorizontalLayoutGroup layout = parent.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = alignment;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return layout;
        }

        public static ContentSizeFitter CreateSizeFitter(Transform parent)
        {
            ContentSizeFitter sizeFitter = parent.gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return sizeFitter;
        }

        public static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public static Font GetFont()
        {
            if (_cachedFont == null)
            {
                _cachedFont = LoadBuiltinFont();

                if (_cachedFont == null)
                {
                    _cachedFont = Font.CreateDynamicFontFromOSFont(
                        new[] { "Arial", "Microsoft YaHei", "SimHei", "Segoe UI" },
                        16);
                }

                if (_cachedFont == null)
                {
                    Debug.LogError("[UiFactory] 无法获取可用字体，请在项目中补充一个可用的 UI 字体资源。");
                }
            }

            return _cachedFont;
        }

        private static Font LoadBuiltinFont()
        {
            string[] builtinFontCandidates =
            {
                "LegacyRuntime.ttf",
                "Arial.ttf"
            };

            foreach (string fontPath in builtinFontCandidates)
            {
                try
                {
                    Font font = Resources.GetBuiltinResource<Font>(fontPath);

                    if (font != null)
                    {
                        return font;
                    }
                }
                catch (ArgumentException)
                {
                    // Ignore and continue trying the next fallback font.
                }
            }

            return null;
        }

        private static void SetTextPadding(RectTransform rectTransform, float padding)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(padding, 4f);
            rectTransform.offsetMax = new Vector2(-padding, -4f);
        }
    }
}
