using OfficeOpenXml;

namespace SSU.DM.ExcelUtils;

public static class ParseTools
{
    public static (string row, string column) GetAddress(ExcelAddress cell)
    {
        var row = string.Join(string.Empty, cell.Address.SkipWhile(char.IsLetter));
        var column = string.Join(string.Empty, cell.Address.TakeWhile(char.IsLetter));

        return (row, column);
    }

    public static int GetLastUsedRow(this ExcelWorksheet worksheet)
    {
        if (worksheet.Dimension == null)
        {
            return 1;
        }

        var row = worksheet.Dimension.End.Row;
        while (row >= 1)
        {
            var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
            if (range.Any(c => !string.IsNullOrEmpty(c.Text) || !string.IsNullOrEmpty(c.Formula)))
            {
                break;
            }

            row--;
        }

        return row;
    }
    
    public static int GetLastUsedColumn(this ExcelWorksheet worksheet)
    {
        if (worksheet.Dimension == null)
        {
            return 1;
        }

        var column = worksheet.Dimension.End.Column;
        while (column >= 1)
        {
            var range = worksheet.Cells[1, column, worksheet.Dimension.End.Row, column];
            if (range.Any(c => !string.IsNullOrEmpty(c.Text) || !string.IsNullOrEmpty(c.Formula)))
            {
                break;
            }

            column--;
        }

        return column;
    }
    
    public static int? GetRowByText(this ExcelWorksheet worksheet, string text)
    {
        var trimmedText = text.Trim();
        foreach (var cell in worksheet.Cells)
        {
            if (cell.Text.Trim().Equals(trimmedText, StringComparison.InvariantCultureIgnoreCase))
            {
                return int.Parse(GetAddress(cell).row);
            }
        }

        return null;
    }
}