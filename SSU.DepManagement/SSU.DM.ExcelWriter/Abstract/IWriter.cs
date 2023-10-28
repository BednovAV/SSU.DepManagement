namespace SSU.DM.ExcelWriter.Abstract;

internal interface IWriter<in TData>
{
    byte[] GetExcel(TData data);
}