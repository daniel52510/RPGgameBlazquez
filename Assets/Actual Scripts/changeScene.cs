using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class changeScene : MonoBehaviour
{
	public static int knightChosen;
	public void loadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void backScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
	public void startGamePaladin()
	{
		knightChosen = 1;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void startGameMage()
	{
		knightChosen = 2;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void startGameBerserker()
	{
		knightChosen = 3;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void goBack()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
	}
}
