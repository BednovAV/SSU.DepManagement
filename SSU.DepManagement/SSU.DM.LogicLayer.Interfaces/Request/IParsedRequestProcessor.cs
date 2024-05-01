using Models.Request;

namespace SSU.DM.LogicLayer.Interfaces.Request;

public interface IParsedRequestProcessor
{
    List<RequestSaveItem> Process(IReadOnlyList<ParsedRequest> parsedRequests);
}