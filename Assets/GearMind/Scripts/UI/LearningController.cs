using UnityEngine;
using UnityEngine.UIElements;

public class LearningController : MonoBehaviour
{
    private UIDocument _doc;
    private VisualElement _root;
    private Image _trainingImage;
    private Label _trainingText;
    private Button _startButton;
    private Button _nextButton;

    private TutorialDataSO _currentTutorial;
    private int _currentSlideIndex = 0;

    void Awake()
    {
        _doc = GetComponent<UIDocument>();

        _root = _doc.rootVisualElement;
        _trainingImage = _root.Q<Image>("TrainingImage");
        _trainingText = _root.Q<Label>("TrainingText");
        _startButton = _root.Q<Button>("StartButton");
        _nextButton = _root.Q<Button>("NextButton");

        _startButton.clicked += Hide;
        _nextButton.clicked += NextSlide;

        _root.style.display = DisplayStyle.None;
    }

    public void Show(TutorialDataSO tutorial)
    {
        _currentTutorial = tutorial;
        _currentSlideIndex = 0;

        ShowCurrentSlide();
        _root.style.display = DisplayStyle.Flex;
    }

    private void ShowCurrentSlide()
    {
        var slide = _currentTutorial.Slides[_currentSlideIndex];

        _trainingImage.image = slide.Image.texture;
        _trainingText.text = slide.Text;

        _nextButton.style.display = (_currentSlideIndex < _currentTutorial.Slides.Count - 1)
            ? DisplayStyle.Flex
            : DisplayStyle.None;

        _startButton.style.display = (_currentSlideIndex == _currentTutorial.Slides.Count - 1)
            ? DisplayStyle.Flex
            : DisplayStyle.None;
    }

    private void NextSlide()
    {
        _currentSlideIndex++;
        if (_currentSlideIndex >= _currentTutorial.Slides.Count)
        {
            Hide();
            return;
        }
        ShowCurrentSlide();
    }

    private void Hide()
    {
        _root.style.display = DisplayStyle.None;
        _currentTutorial = null;
        _currentSlideIndex = 0;
    }

    private void OnDisable()
    {
        _startButton.clicked -= Hide;
        _nextButton.clicked -= NextSlide;
    }
}