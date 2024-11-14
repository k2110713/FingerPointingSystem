using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class CircleButton : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private bool wasInCircle = false;
    public GameObject pointer;

    // 色の設定
    [SerializeField] private Color defaultColor = Color.white;      // デフォルトの色
    [SerializeField] private Color enterColor = Color.green;         // マウスが乗ったときの色
    [SerializeField] private Color clickColor = Color.red;           // クリック時の色

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = defaultColor; // 初期色をデフォルト色に設定
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsPointerInCircle())
        {
            GetComponent<Button>().onClick.Invoke();
            Debug.Log("click");

            // クリック時の色に変更
            image.color = clickColor;
        }
    }

    private void Update()
    {
        // 現在のマウス位置を取得し、円形の範囲内にあるかどうかを確認
        bool isInCircle = IsPointerInCircle();

        if (!wasInCircle && isInCircle)
        {
            // 前のフレームでは範囲外だったが、現在のフレームでは範囲内の場合
            Debug.Log("enter");

            // マウスがボタンの上に来たときの色に変更
            image.color = enterColor;
        }
        else if (wasInCircle && !isInCircle)
        {
            // 前のフレームでは範囲内だったが、現在のフレームでは範囲外の場合
            Debug.Log("exit");

            // マウスがボタンから離れたらデフォルト色に戻す
            image.color = defaultColor;
        }

        // 状態を更新
        wasInCircle = isInCircle;
    }

    private bool IsPointerInCircle()
    {
        // ボタンとポインタのローカル位置を計算
        RectTransform rectTransform = image.rectTransform;
        Vector2 buttonCenter = rectTransform.localPosition; // ボタンの中心位置
        Vector2 localPointerPosition = pointer.transform.localPosition; // ポインタの位置

        // ポインタからボタン中心への相対位置
        Vector2 relativePosition = localPointerPosition - buttonCenter;
        float radius = rectTransform.rect.width * 0.42f;

        Debug.Log("Pointer: " + localPointerPosition + ", RelativePosition: " + relativePosition.magnitude + ", Radius: " + radius);

        // 円の範囲内にいるかを判定
        return relativePosition.magnitude < radius;
    }
}
