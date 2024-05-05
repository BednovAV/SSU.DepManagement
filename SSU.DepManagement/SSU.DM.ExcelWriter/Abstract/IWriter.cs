using OfficeOpenXml;

namespace SSU.DM.ExcelWriter.Abstract;

internal interface IWriter<in TData>
{
    ExcelPackage GetExcel(TData data);
}