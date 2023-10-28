using System.ComponentModel;
using OfficeOpenXml;
using SSU.DM.ExcelParser.Exceptions;
using SSU.DM.ExcelUtils;

namespace SSU.DM.ExcelParser.Abstract;

internal abstract class AbstractParser<TMappedResult, TLoadData> : IParser<TMappedResult>
    where TMappedResult : class
{
    private const string SOME_VALUE = "--//--";
    private readonly Dictionary<string, object?> _lastValues = new();

    public TMappedResult? Parse(byte[] bytes)
    {
        using var excelDoc = bytes.ReadExcel();
        var loadedData = TryLoadData(excelDoc.Workbook);

        return loadedData == null ? null
            : TryMapToResult(loadedData);
    }

    private TLoadData TryLoadData(ExcelWorkbook workBook)
    {
        TLoadData? loadedData;
       // try
        //{
            loadedData = LoadData(workBook);
        //}
        // catch (Exception e)
        // {
        //     throw new DataLoadException(e);
        // }

        if (loadedData == null)
            throw new DataLoadException();

        return loadedData;
    }

    private TMappedResult TryMapToResult(TLoadData loadedData)
    {
        TMappedResult? mappedResult;
        // try
        // {
        mappedResult = MapToResult(loadedData);
        // }
        // catch (Exception e)
        // {
        //
        //     throw new MapToResultException(e);
        // }

        if (mappedResult == null)
            throw new MapToResultException();

        return mappedResult;
    }

    protected abstract TLoadData? LoadData(ExcelWorkbook workBook);
    protected abstract TMappedResult? MapToResult(TLoadData loadedData);

    // protected TEnum ProcessEnumField<TEnum>(string value, string column) where TEnum : struct
    //     => typeof(TEnum).IsEnum ? GetField(GetEnumValueFromDescription<TEnum>(value), column) : default;

    protected TValue? GetField<TValue>(ExcelRange cell)
    {
        var (row, column) = GetAddress(cell);
        if (cell.Text == SOME_VALUE)
        {
            if (_lastValues.TryGetValue(column, out var lastValue))
                return (TValue?)lastValue;
            return default;
        }
        
        
        TValue? cellValue;
        if (typeof(TValue).IsEnum)
            cellValue = GetEnumValueFromDescription<TValue>(cell.Text);
        // else if (typeof(TValue) != typeof(DateTime) && cell.IsDateTime)
        //     cellValue = (TValue)(object)cell.DateTimeValue.Value.ToString("dd.MM.yy");
        // if (typeof(TValue) == typeof(string))
        //     cellValue = (TValue)(object)value.ToString();
        else if (typeof(TValue) == typeof(string))
            cellValue = (TValue)(object)cell.Text;
        else
             cellValue = cell.GetValue<TValue>();
        _lastValues[column] = cellValue;
        return cellValue;
    }

    private (string row, string column) GetAddress(ExcelAddress cell)
    {
        var row = string.Join(string.Empty, cell.Address.SkipWhile(char.IsLetter));
        var column = string.Join(string.Empty, cell.Address.TakeWhile(char.IsLetter));

        return (row, column);
    }

    private T GetEnumValueFromDescription<T>(string description)
    {
        var fields = typeof(T).GetFields();
        foreach (var fieldInfo in fields)
        {
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes is { Length: > 0 } && attributes[0].Description == description)
                return (T)Enum.Parse(typeof(T), fieldInfo.Name);
        }
        return default!;
    }
}
