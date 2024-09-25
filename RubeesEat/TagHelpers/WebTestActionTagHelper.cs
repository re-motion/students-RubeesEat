using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RubeesEat.TagHelpers;

[HtmlTargetElement(Attributes = "t-action")]
public class WebTestActionTagHelper : TagHelper
{ 
    private readonly IWebHostEnvironment _environment;

    [HtmlAttributeName("t-action")]
    public string Action { get; set; }

    [HtmlAttributeName("t-click-behavior")]
    public string ClickBehavior { get; set; } = "ClickAndWaitUntilStale";

    public WebTestActionTagHelper(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_environment.IsProduction())
        {
            return;
        }
        output.Attributes.Add("data-test-action", Action);
        output.Attributes.Add("data-test-click-behavior", ClickBehavior);
    }
}
