using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int GridRows = 2;
    public const int GridCols = 4;
    public const float OffsetX = 2f;
    public const float OffsetY = 2.5f;
    public bool CanReveal => secondRevealed == null;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TMP_Text scoreLabel;
    
    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;
    private int score = 0;

    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealed != null && secondRevealed != null) return;
        
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;

            StartCoroutine(CheckMatch());
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Start()
    {
        var startingPosition = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        numbers = ShuffleArray(numbers);

        for (var i = 0; i < GridCols; i++)
        {
            for (var j = 0; j < GridRows; j++)
            {
                MemoryCard card;

                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                var index = j * GridCols + i;
                var id = numbers[index];
                card.SetCard(id, images[id]);

                var positionX = (OffsetX * i) + startingPosition.x;
                var positionY = -(OffsetY * j) + startingPosition.y;

                card.transform.position = new Vector3(positionX, positionY, startingPosition.z);
            }
        }
    }

    // Knuth shuffle algorithm
    private static int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }

    private IEnumerator<YieldInstruction> CheckMatch()
    {
        if (firstRevealed.Id == secondRevealed.Id)
        {
            score++;
            scoreLabel.text = $"Score: {score}";
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            
            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }

        firstRevealed = null;
        secondRevealed = null;
    }
}
