using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class SimpleSpriteAnimator : MonoBehaviour
{
    private Sprite sprite1;
    private Sprite sprite2;
    private float interval = 0.5f;
    private Image targetImage;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
        if (targetImage != null) targetImage.preserveAspect = true; // 비율 유지 강제
    }

    public void SetAnimationData(Sprite s1, Sprite s2, float time)
    {
        sprite1 = s1;
        sprite2 = s2;
        interval = time;
        if (gameObject.activeInHierarchy) StartAnim();
    }

    private void OnEnable() => StartAnim();

    private void StartAnim()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        if (sprite1 != null && sprite2 != null)
            animationCoroutine = StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        while (true)
        {
            if (targetImage != null) targetImage.sprite = sprite1;
            yield return new WaitForSeconds(interval);
            if (targetImage != null) targetImage.sprite = sprite2;
            yield return new WaitForSeconds(interval);
        }
    }
}