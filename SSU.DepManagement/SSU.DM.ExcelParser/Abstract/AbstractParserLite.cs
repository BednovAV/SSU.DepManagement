using OfficeOpenXml;

namespace SSU.DM.ExcelParser.Abstract;

internal abstract class AbstractParserLite<TMappedResult>
    : AbstractParser<TMappedResult, TMappedResult>
    where TMappedResult : class
{
    protected override TMappedResult? LoadData(ExcelWorkbook workBook)
    {
        return MapToResult(workBook);
    }

    protected override TMappedResult? MapToResult(TMappedResult loadedData)
    {
        return loadedData;
    }

    protected abstract TMappedResult? MapToResult(ExcelWorkbook workBook);
}
