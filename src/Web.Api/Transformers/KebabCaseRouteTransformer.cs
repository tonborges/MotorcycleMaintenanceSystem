using System.Text.RegularExpressions;

namespace Web.Api.Transformers;

public class KebabCaseRouteTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object? value)
    {
        return value is null ?
            "" : 
            Regex.Replace(value?.ToString() ?? "", "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
