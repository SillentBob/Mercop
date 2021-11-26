using System.Collections.Generic;
using Mercop.Core;
using Mercop.Ui;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsView : View
{
    // @formatter:off
    [SerializeField] private Leaderboards leaderboardsRoot;
     [SerializeField] private ScrollRect scrollList;
    [SerializeField] private RectTransform rowsContentContainer;
    [SerializeField] private RectTransform rowsContentAnchor;
    [SerializeField] private RectTransform contentRowPrefab;
    [SerializeField] private Button backButton;
    
    [SerializeField] private int rowsCount = 100;
    [SerializeField] private int visibleCount = 10;
    // @formatter:on

    private int rowsBufferTop = 2;
    private int rowsBufferBottom = 2;
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
        var visibleRowsCountWithBuffer = visibleCount + rowsBufferTop + rowsBufferBottom;
        rows = new List<RectTransform>(visibleRowsCountWithBuffer);
        rowHeight = Mathf.Abs(rowsContentContainer.sizeDelta.y / visibleCount);
        Debug.Log($"RD {rowsContentContainer.sizeDelta} RS {rowsContentContainer.rect.size.y}");
        rowsContentAnchor.sizeDelta = new Vector2(rowsContentAnchor.rect.size.x, rowsCount * rowHeight);

        // extra, above and below for smooth looks
        for (int i = -rowsBufferTop; i < visibleRowsCountWithBuffer - rowsBufferTop; i++)
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

    private void OnScroll(Vector2 delta)
    {
        var pixelsScrolled = rowsContentContainer.localPosition.y;
        while (lastViewPosition + rowHeight < pixelsScrolled)
        {
            MoveTopOffscreenRowToBottom();
        }

        while (lastViewPosition - rowHeight > pixelsScrolled)
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
        var anchoredPosition = row.anchoredPosition;
        var currentY = anchoredPosition.y;
        var currentX = anchoredPosition.x;
        var newAnchoredPos = new Vector2(currentX, currentY + (rows.Count * rowHeight));
        anchoredPosition = newAnchoredPos;
        row.anchoredPosition = anchoredPosition;
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
        EnableMenu(true);
    }

    public override void OnHide()
    {
        EnableMenu(false);
    }
}