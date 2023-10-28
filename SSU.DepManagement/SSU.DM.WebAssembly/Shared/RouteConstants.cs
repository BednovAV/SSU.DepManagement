﻿namespace SSU.DM.WebAssembly.Shared;

public static class RouteConstants
{
    public const string IMPORT_REQUEST_GET_APP_FORMS = "importrequest/getappforms";
    public const string IMPORT_REQUEST_GET_FILE = "importrequest/getfile";
    public const string IMPORT_REQUEST_UPLOAD_REQUEST = "importrequest/uploadrequest";
    public const string IMPORT_REQUEST_CHECK_UPLOAD = "importrequest/checkupload";
    public const string IMPORT_REQUEST_DELETE_APP_FORM = "importrequest/deleteappform";
    
    public const string REPORTS_CALCULATION_OF_HOURS = "reports/calculationofhours";
    
    public const string FACULTY = "faculty";

    /// <summary>
    /// Adds the specified parameter to the Query String.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="paramName">Name of the parameter to add.</param>
    /// <param name="paramValue">Value for the parameter to add.</param>
    /// <returns>Url with added parameter.</returns>
    public static string AddParameter<T>(this string url, string paramName, T paramValue)
    {
        var uriBuilder = new UriBuilder(url);
        var separator = string.IsNullOrEmpty(uriBuilder.Query) ? "?" : "&";
        return $"{url}{separator}{paramName}={paramValue.ToString()}";
    }

    public static string AddParameter(this string url, string paramName, IEnumerable<string> paramValue)
    {
        return paramValue
            ?.Aggregate(url, (aggregateUrl, value) => aggregateUrl.AddParameter(paramName, value))
            ?? string.Empty;
    }

    public static string GetUrl(this HttpClient client, string methodUrl)
        => client.BaseAddress + methodUrl;
}
