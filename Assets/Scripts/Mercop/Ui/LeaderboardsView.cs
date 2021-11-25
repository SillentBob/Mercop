using System.Collections.Generic;
using Mercop.Core;
using Mercop.Ui;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LeaderboardsView : View
{
    [SerializeField] private Leaderboards leaderboardsRoot;
    [SerializeField] private int rowsCount = 100;
    [SerializeField] private int visibleCount = 10;
    [FormerlySerializedAs("contentContainer")] [SerializeField]
    private RectTransform rowsContentContainer;
    [FormerlySerializedAs("contentAnchor")] [SerializeField]
    private RectTransform rowsContentAnchor;
    [SerializeField] private RectTransform contentRowPrefab;
    [SerializeField] private ScrollRect scrollList;
    [SerializeField] private Button backButton;

    private float rowHeight;
    private float lastViewPosition;
    private List<RectTransform> rows;

    private void Awake()
    {
        SetupList();
        RegisterScrollEvents();
        RegisterButtonEvents();
    }

    private void SetupList()
    {
        var visibleRowsCountWithBuffer = visibleCount + 2;
        rows = new List<RectTransform>(visibleRowsCountWithBuffer);
        rowHeight = contentRowPrefab.rect.size.y;
        rowsContentAnchor.sizeDelta = new Vector2(rowsContentAnchor.rect.size.x, rowsCount * rowHeight);

        for (int i = -1; i < visibleRowsCountWithBuffer-1; i++) //+2 extra, above and below for smooth pooling
        {
            RectTransform row = Instantiate(contentRowPrefab, rowsContentAnchor);
            row.anchoredPosition = new Vector2(0, -i * rowHeight);
            rows.Add(row);
        }

        lastViewPosition = 0;
    }

    private void RegisterScrollEvents()
    {
        scrollList.onValueChanged.AddListener(OnScroll);
    }

    private void RegisterButtonEvents()
    {
        backButton.onClick.AddListener(ViewManager.Instance.PreviousView);
    }

    private void OnScroll(Vector2 deltaPosition)
    {
        var pixelsScrolled = rowsContentContainer.localPosition.y;
        if (lastViewPosition + rowHeight < pixelsScrolled)
        {
            MoveTopOffscreenRowToBottom();
        }

        if (lastViewPosition - rowHeight > pixelsScrolled)
        {
            MoveBottomOffscreenRowToTop();
        }
    }

    private void MoveTopOffscreenRowToBottom()
    {
        var row = rows[0];
        var anchoredPosition = row.anchoredPosition;
        var currentY = anchoredPosition.y;
        var currentX = anchoredPosition.x;
        var newAnchoredPos = new Vector2(currentX, currentY - (rows.Count * rowHeight));
        anchoredPosition = newAnchoredPos;
        row.anchoredPosition = anchoredPosition;
        lastViewPosition += rowHeight;
        MoveItemAtIndexTo(0, rows.Count - 1);
        row.transform.SetSiblingIndex(rows.Count);
    }
    
    private void MoveBottomOffscreenRowToTop()
    {
        var row = rows[rows.Count - 1];
        var currentY = row.anchoredPosition.y;
        var currentX = row.anchoredPosition.x;
        var newAnchoredPos = new Vector2(currentX, currentY + (rows.Count * rowHeight));
        row.anchoredPosition = newAnchoredPos;
        lastViewPosition -= rowHeight;
        MoveItemAtIndexTo(rows.Count - 1, 0);
        row.transform.SetSiblingIndex(0);
    }

    private void UpdateMovedRowValues(RectTransform row)
    {
        
    }

    private void MoveItemAtIndexTo(int from, int to)
    {
        var item = rows[from];
        rows.RemoveAt(from);
        rows.Insert(to, item);
    }

    private void EnableMenu(bool enable)
    {
        leaderboardsRoot.gameObject.SetActive(enable);
    }

    public override void OnShow()
    {
        Debug.Log("LB onshow");
        EnableMenu(true);
    }

    public override void OnHide()
    {
        EnableMenu(false);
    }
}