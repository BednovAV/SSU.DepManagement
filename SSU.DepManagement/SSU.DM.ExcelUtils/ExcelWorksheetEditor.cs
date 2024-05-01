using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SSU.DM.ExcelUtils;

public class ExcelWorksheetEditor
{
    private readonly ExcelWorksheet _worksheet;
    private readonly Regex _markRegex;

    public ExcelWorksheetEditor(ExcelWorksheet worksheet)
    {
        _worksheet = worksheet;
        _markRegex =  new Regex(@"{\w+}", RegexOptions.Compiled);
    }

    public void Merge(string addressRange,
        string? insertValue = null,
        bool bold = false,
        bool italic = false,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Left)
    {
        var cellsRange = _worksheet.Cells[addressRange];
        cellsRange.Merge = true;
        cellsRange.Value = insertValue ?? string.Empty;
        ApplyStandardStyle(cellsRange.Style, bold, italic);
    }

    public void SetValueRightAlignment(string address,
        object? text,
        bool bold = false,
        bool italic = false)
        => SetValue(address, text, bold, italic, ExcelHorizontalAlignment.Right);
    
    public void SetFormula(string address,
        string formula,
        bool bold = false,
        bool italic = false,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Right)
    {
        var cell = _worksheet.Cells[address];
        cell.Formula = formula;
        ApplyStandardStyle(cell.Style, bold, italic, horizontalAlignment, wrapText: true);
    }
    
    public void SetValue(string address,
        object? text,
        bool bold = false,
        bool italic = false,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Left)
    {
        var cell = _worksheet.Cells[address];
        cell.Value = text;
        ApplyStandardStyle(cell.Style, bold, italic, horizontalAlignment, wrapText: true);
    }
    
    public void SetStandardTableStyle(string addressRange)
    {
        var cellsRange = _worksheet.Cells[addressRange];
        for (var i = cellsRange.Start.Row; i <= cellsRange.End.Row; i++)
        {
            for (var j = cellsRange.Start.Column; j <= cellsRange.End.Column; j++)
            {
                _worksheet.Cells[i, j].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }
    }
    
    public void SetBorders(ExcelBorderStyle borderStyle, string? addressRange = null)
    {
        var cellsRange = addressRange != null
            ? _worksheet.Cells[addressRange]
            : _worksheet.Cells[1, 1, GetLastUsedRow(), GetLastUsedColumn()];

        cellsRange.Style.Border.Top.Style = borderStyle;
        cellsRange.Style.Border.Bottom.Style = borderStyle;
        cellsRange.Style.Border.Right.Style = borderStyle;
        cellsRange.Style.Border.Left.Style = borderStyle;
    }
    
    public void ReplacePlaceholders(IDictionary<string, string> placeholders)
    {
        foreach (var cell in _worksheet.Cells)
        {
            var matches = _markRegex.Matches(cell.Text);
            foreach (Match match in matches)
            {
                if (placeholders.TryGetValue(match.Value.Trim('{', '}'), out var newValue))
                {
                    SetValue(cell.Address,cell.Text.Replace(match.Value, newValue));
                }
            }
        }
    }
    
    public int? GetRowByMark(string mark)
    {
        foreach (var cell in _worksheet.Cells)
        {
            var matches = _markRegex.Matches(cell.Text);
            foreach (Match match in matches)
            {
                if (match.Value.Trim('{', '}').Equals(mark, StringComparison.InvariantCultureIgnoreCase))
                {
                    return int.Parse(ParseTools.GetAddress(cell).row);
                }
            }
        }

        return null;
    }

    public void CopyFrom(
        ExcelWorksheetEditor sourceWorksheet,
        string destination,
        params ExcelRangeCopyOptionFlags[] excelRangeCopyOptionFlags)
    {
        CopyFrom(sourceWorksheet._worksheet, destination, excelRangeCopyOptionFlags);
    }
    
    public void CopyFrom(
        ExcelWorksheet sourceWorksheet,
        string destination,
        params ExcelRangeCopyOptionFlags[] excelRangeCopyOptionFlags)
    {
        sourceWorksheet.Cells[1, 1, sourceWorksheet.GetLastUsedRow(), sourceWorksheet.GetLastUsedColumn()]
            .Copy(_worksheet.Cells[destination], excelRangeCopyOptionFlags);

        for (int i = 1; i < sourceWorksheet.GetLastUsedColumn() + 1; i++)
        {
            _worksheet.Columns[i].Width = sourceWorksheet.Columns[i].Width;
        }
        
        for (int i = 1; i < sourceWorksheet.GetLastUsedRow() + 1; i++)
        {
            _worksheet.Rows[i].Height = sourceWorksheet.Rows[i].Height;
        }
    }
    
    public int GetLastUsedRow()
    {
        return _worksheet.GetLastUsedRow();
    }
    
    public int GetLastUsedColumn()
    {
        return _worksheet.GetLastUsedColumn();
    }

    private void ApplyStandardStyle(ExcelStyle style,
        bool bold = false,
        bool italic = false,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Left,
        bool wrapText = false)
    {
        style.WrapText = wrapText;
        style.Font.Size = 10;
        style.Font.Name = "Times New Roman";
        style.Font.Bold = bold;
        style.Font.Italic = italic;
        style.HorizontalAlignment = horizontalAlignment;
    }

    public void InsertEmptyRows(int currentRow, int count)
    {
        _worksheet.InsertRow(currentRow, count);
    }

    public void Clear()
    {
        _worksheet.Cells.Delete(eShiftTypeDelete.Up);
    }

    public int FilledRowsCount()
    {
        return _worksheet.GetLastUsedRow();
    }
}
