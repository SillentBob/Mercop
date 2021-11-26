using System.Collections.Generic;
using Mercop.Core;
using Mercop.Ui;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsView : View
{
    // @formatter:off
    [SerializeField] private Leaderboards leaderboardsRoot;
    [SerializeField] private RectTransform list;
    [SerializeField] private ScrollRect scrollRect;
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
    private bool scrollListUpdated;

    private void Awake()
    {
        RegisterScrollEvents();
        RegisterButtonEvents();
    }

    private void RegisterScrollEvents()
    {
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void RegisterButtonEvents()
    {
        backButton.onClick.AddListener(ViewManager.Instance.PreviousView);
    }

    private void OnScroll(Vector2 delta)
    {
        var pixelsScrolled = scrollRect.content.localPosition.y;
        
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

    /// <summary>
    /// This is mandatory becouse UI problems - must be called after scrollList was enabled
    /// </summary>
    private void BuildScrollListContentOnce()
    {
        if (!scrollListUpdated)
        {
            RebuildScrollListContent();
            scrollListUpdated = true;
        }
    }

    private void RebuildScrollListContent()
    {
        var visibleRowsCountWithBuffer = visibleCount + rowsBufferTop + rowsBufferBottom;
        rows = new List<RectTransform>(visibleRowsCountWithBuffer);

        rowHeight = list.rect.height / visibleCount;
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, rowHeight * rowsCount);

        // extra, above and below for smooth looks
        for (int i = -rowsBufferTop; i < visibleRowsCountWithBuffer - rowsBufferTop; i++)
        {
            RectTransform row = Instantiate(contentRowPrefab, scrollRect.content);
            row.sizeDelta = new Vector2(row.sizeDelta.x, rowHeight);
            row.anchoredPosition = new Vector2(0, -i * rowHeight);
            rows.Add(row);
        }

        lastViewPosition = 0;
    }

    public override void OnShow()
    {
        EnableMenu(true);
        BuildScrollListContentOnce();
    }

    public override void OnHide()
    {
        EnableMenu(false);
    }
}