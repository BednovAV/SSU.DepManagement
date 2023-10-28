using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SSU.DM.ExcelUtils;

public class ExcelWorksheetEditor
{
    private readonly ExcelWorksheet _worksheet;

    public ExcelWorksheetEditor(ExcelWorksheet worksheet)
    {
        _worksheet = worksheet;
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
        object text,
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
        object text,
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
}
