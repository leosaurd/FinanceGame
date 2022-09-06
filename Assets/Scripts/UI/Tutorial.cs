using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	public static Tutorial Instance { get; private set; }

	private CanvasGroup canvasGroup;

	public Page[] pages;

	public TextMeshProUGUI pageText;

	public TextMeshProUGUI subtitle;
	public TextMeshProUGUI description;
	public Image image;
	public Image gif;

	public Image previousImage;

	private int currentPage = 0;

	public int CurrentPage
	{
		get { return currentPage; }
		set
		{
			currentPage = value;

			pageText.text = (currentPage + 1) + "/" + pages.Length;

			if (currentPage == 0) previousImage.color = new Color(0.1f, 0.22f, 0.4f);
			else previousImage.color = new Color(1, 1, 1);


			if (currentPage == pages.Length)
			{
				Close();
			}
			else
			{
				LoadPage();
			}
		}
	}

	private void Awake()
	{
		if (Instance == null)
			Instance = this;

		canvasGroup = GetComponent<CanvasGroup>();

		pageText = transform.Find("PageCount").GetComponent<TextMeshProUGUI>();

		subtitle = transform.Find("Subtitle").GetComponent<TextMeshProUGUI>();
		description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
		image = transform.Find("Image").GetComponent<Image>();
		gif = transform.Find("GIF").GetComponent<Image>();

		previousImage = transform.Find("BackButton").GetComponentInChildren<Image>();

		CurrentPage = 0;

		if (SettingsManager.Instance.seenTutorial == false) Open();
	}

	public void LoadPage()
	{
		Page page = pages[currentPage];

		subtitle.text = page.title;
		description.text = page.description;
		if (currentPage == 0)
		{
			image.gameObject.SetActive(false);
			gif.gameObject.SetActive(true);
			gif.GetComponent<Gif>().StartLoop();
		}
		else
		{
			image.gameObject.SetActive(true);
			gif.gameObject.SetActive(false);
			image.sprite = page.Image;
		}

	}


	IEnumerator FadeIn()
	{
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;

		int steps = 30;
		for (int i = 0; i <= steps; i++)
		{
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator FadeOut()
	{
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;

		int steps = 30;
		for (int i = steps; i >= 0; i--)
		{
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}

		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
	}

	public void Open()
	{
		SettingsManager.Instance.seenTutorial = true;
		StartCoroutine(FadeIn());
	}

	public void Close()
	{
		StartCoroutine(FadeOut());
		CurrentPage = 0;
	}

	public void Forward()
	{
		CurrentPage++;
	}

	public void Previous()
	{
		if (CurrentPage != 0)
			CurrentPage--;
	}
}

[System.Serializable]
public class Page
{
	public string title;
	public string description;
	public Sprite Image;

	public Page(string title, string description, Sprite image)
	{
		this.title = title;
		this.description = description;
		Image = image;
	}
}