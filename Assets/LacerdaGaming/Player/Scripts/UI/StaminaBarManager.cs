using UnityEngine;
using UnityEngine.UI;

public class StaminaBarManager : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private PlayerStateMachine playerStateMachine;

    private float HideCounter;
    private bool isHiding;

    private const float COUNTER_MAX = .5f;

    private void Start()
    {
        Hide();
        HideCounter = COUNTER_MAX;
        playerStateMachine.OnStaminaChange += PlayerStateMachine_OnStaminaChange;
    }

    private void Update()
    {
        if (isHiding)
        {
            CountToHide();
        }
    }

    private void PlayerStateMachine_OnStaminaChange(object sender, PlayerStateMachine.OnStaminaChangeEventArgs e)
    {
        progressImage.fillAmount = .99f;
        progressImage.fillAmount = e.stamina / 100;

        if (progressImage.fillAmount >= 1)
        {
            isHiding = true;
        }
        else
        {
            Show();
        }
    }

    private void CountToHide()
    {
        if (HideCounter > 0f)
            HideCounter -= Time.deltaTime;

        if (HideCounter <= 0f)
        {
            Hide();
            HideCounter = COUNTER_MAX;
            isHiding = false;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
